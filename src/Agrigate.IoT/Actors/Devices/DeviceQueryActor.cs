using Agrigate.Domain.DTOs;
using Agrigate.Domain.DTOs.IoT;
using Agrigate.Domain.Messages.IoT;
using Agrigate.IoT.Domain.Contexts;
using Agrigate.IoT.Domain.Entities;
using Agrigate.IoT.Domain.Messages;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// An actor responsible for interacting with the IoT Service Database on 
/// behalf of Device actors
/// </summary>
public class DeviceQueryActor : ReceiveActor
{
    private readonly IDbContextFactory<IoTContext> _dbFactory;

    public DeviceQueryActor(IDbContextFactory<IoTContext> dbFactory)
    {
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));

        Receive<DeviceConnect>(HandleDeviceConnect);
        Receive<DeviceRetrieval>(HandleDeviceRetrieval);
        Receive<TelemetryReceived>(HandleTelemetryReceived);
        Receive<GetTelemtry>(HandleTelemetryRetrieval);
    }

    /// <summary>
    /// Attempts to retrieve a device based on a deviceId if it exists, 
    /// otherwise creates a new device
    /// </summary>
    /// <param name="message">A message containing a deviceId</param>
    private void HandleDeviceConnect(DeviceConnect message)
    {
        var connectionTime = DateTimeOffset.UtcNow;
        using var db = _dbFactory.CreateDbContext();

        var device = db.Devices
            .FirstOrDefault(device => device.DeviceId == message.DeviceId);
        device ??= db.Devices
            .Add(new Domain.Entities.Device 
            {
                DeviceId = message.DeviceId,
                DeviceKey = Guid.NewGuid(),
                Created = connectionTime,
            }).Entity;

        device.LastConnection = connectionTime;
        device.Modified = connectionTime;
        db.SaveChanges();

        Sender.Tell(device);
        Self.Tell(PoisonPill.Instance);
    }

    /// <summary>
    /// Retrieves all devices that have connected to Agrigate, and their current
    /// online status
    /// </summary>
    /// <param name="message">A message containing the currently online 
    /// devices</param>
    private void HandleDeviceRetrieval(DeviceRetrieval message)
    {
        var results = new List<DeviceWithStatus>();
        using var db = _dbFactory.CreateDbContext();

        if (message.DeviceId.IsNullOrEmpty())
        {
            var allDevices = db.Devices.AsNoTracking().ToList();
            foreach (var device in allDevices)
            {
                var onlineDevice = message.ActiveDevices.Contains(device.DeviceId);
                results.Add(new DeviceWithStatus
                {
                    DeviceId = device.DeviceId,
                    LastConnection = device.LastConnection,
                    Online = onlineDevice
                });
            }

            Sender.Tell(results);
        }
        else
        {
            var device = db.Devices
                .AsNoTracking()
                .Include(d => d.Methods)
                .FirstOrDefault(d => d.DeviceId == message.DeviceId);
            
            if (device == null)
                Sender.Tell(new DataNotFound());

            else
            {
                var result = new DeviceDetails
                {
                    DeviceId = device.DeviceId,
                    LastConnection = device.LastConnection,
                    Model = device.Model,
                    SerialNumber = device.SerialNumber,
                    Online = message.ActiveDevices.Contains(device.DeviceId),
                    Methods = device.Methods?.Select(m => new MethodDetails
                    {
                        Name = m.Name,
                        Description = m.Description,
                        Parameters = null
                    }).ToList()
                };

                Sender.Tell(result);
            }
        }

        Self.Tell(PoisonPill.Instance);
    }

    /// <summary>
    /// Records telemetry received by a device
    /// </summary>
    /// <param name="message">A message containing the telemetry to save</param>
    private void HandleTelemetryReceived(TelemetryReceived message) 
    {
        using var db = _dbFactory.CreateDbContext();

        var device = db.Devices.AsNoTracking()
            .FirstOrDefault(device => device.DeviceId == message.DeviceId);
        
        if (device == null)
            return;

        var now = DateTimeOffset.UtcNow;
        var newTelemetry = new List<Telemetry>();
        foreach(var telemetry in message.Telemetry)
        {
            newTelemetry.Add(new Telemetry
            {
                DeviceId = device.Id,
                Timestamp = telemetry.Timestamp,
                Key = telemetry.Key,
                Value = telemetry.Value,
                Created = now,
                Modified = now
            });
        }

        db.AddRange(newTelemetry);
        db.SaveChanges();
        
        Self.Tell(PoisonPill.Instance);
    }

    /// <summary>
    /// Retrieves telemetry for the specified device
    /// </summary>
    /// <param name="message">A message containing the DeviceId for which to 
    /// retrieve telemetry</param>
    public void HandleTelemetryRetrieval(GetTelemtry message)
    {
        using var db = _dbFactory.CreateDbContext();

        var device = db.Devices.AsNoTracking()
            .FirstOrDefault(device => device.DeviceId == message.DeviceId);

        if (device == null)
            Sender.Tell(new DataNotFound());
        
        else
        {
            var startTime = message.StartTime 
                ?? DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(-7));

            var query = db.Telemetry
                .Where(t => 
                    t.DeviceId == device.Id
                    && t.Timestamp > startTime
                );

            if (message.EndTime.HasValue)
                query = query.Where(t => t.Timestamp < message.EndTime.Value);

            var telemetry = query.OrderBy(t => t.Timestamp).ToList();

            var result = new Agrigate.Domain.DTOs.IoT.DeviceTelemetry
            {
                DeviceId = device.DeviceId,
                Telemetry = telemetry.Select(t => new TelemetryData
                {
                    Timestamp = t.Timestamp,
                    Key = t.Key,
                    Value = t.Value ?? 0
                }).ToList()
            };

            Sender.Tell(result);
        }
        
        Self.Tell(PoisonPill.Instance);
    }
}