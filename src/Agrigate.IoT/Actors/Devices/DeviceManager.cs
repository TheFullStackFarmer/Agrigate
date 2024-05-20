using Akka.Actor;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// The actor responsible for managing devices connected to Agrigate
/// </summary>
public class DeviceManager : ReceiveActor
{
    public DeviceManager()
    {
    }

    // TODO: Connect to MQTT broker
    // TODO: Listen to connection events
    // TODO: When new connection occurs, create a device actor
    // TODO: When client disconnects, kill device actor
}