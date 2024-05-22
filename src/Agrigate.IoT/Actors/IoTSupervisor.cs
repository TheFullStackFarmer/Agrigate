using Agrigate.Domain.Messages;
using Agrigate.IoT.Actors.Devices;
using Akka.Actor;
using Akka.DependencyInjection;
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
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");

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

        var deviceManagerProps = DependencyResolver.For(Context.System).Props<DeviceManager>();
        _deviceManager = Context.ActorOf(deviceManagerProps, "DeviceManager");

        _log.Info($"{nameof(CreateManagers)} completed!");        
    }

    private void Ping(object message)
    {
        _log.Info($"{nameof(IoTSupervisor)} received message: {message}");
        Sender.Tell("Ping.Pong!");
    }
}