using Agrigate.Domain.DTOs.IoT;
using Agrigate.IoT.Domain.Contexts;
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
    }

    /// <summary>
    /// Attempts to retrieve a device based on a deviceId if it exists, 
    /// otherwise creates a new device
    /// </summary>
    /// <param name="message">A message containing a deviceId</param>
    public void HandleDeviceConnect(DeviceConnect message)
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
    public void HandleDeviceRetrieval(DeviceRetrieval message)
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
}