using Agrigate.Domain.DTOs.IoT;
using Agrigate.IoT.Domain.Contexts;
using Agrigate.IoT.Domain.Entities;
using Agrigate.IoT.Domain.Messages;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;

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
}