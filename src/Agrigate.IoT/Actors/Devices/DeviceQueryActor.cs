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
}