using Agrigate.Domain.Messages;
using Agrigate.IoT.Actors.Devices;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.IoT.Actors;

/// <summary>
/// The actor responsible for supervising the entire Agrigate IoT service
/// </summary>
public class IoTSupervisor : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private IActorRef? _deviceManager;

    public IoTSupervisor()
    {
        _log = Logging.GetLogger(Context);
        Receive<TestMessage>(Ping);
    }

    protected override void PreStart()
    {
        _log.Info($"{nameof(IoTSupervisor)} starting...");
        CreateManagers();
    }

    private void CreateManagers()
    {
        _log.Info($"{nameof(CreateManagers)} running...");

        _deviceManager = Context.ActorOf(
            Props.Create(() => new DeviceManager()),
            "DeviceManager"
        );

        _log.Info($"{nameof(CreateManagers)} completed!");        
    }

    private void Ping(object message)
    {
        _log.Info($"{nameof(IoTSupervisor)} received message: {message}");
        Sender.Tell("Ping.Pong!");
    }
}