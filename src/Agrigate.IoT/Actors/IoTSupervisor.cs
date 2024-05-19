using Akka.Actor;

namespace Agrigate.IoT.Actors;

/// <summary>
/// The actor responsible for supervising the entire Agrigate IoT service
/// </summary>
public class IoTSupervisor : ReceiveActor
{
    public IoTSupervisor()
    {
    }
}