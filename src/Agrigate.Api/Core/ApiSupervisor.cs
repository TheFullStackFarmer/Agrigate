using Agrigate.Domain.Messages;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.Api.Core;

/// <summary>
/// The actor responsible for supervising the Agrigate API actors
/// </summary>
public class ApiSupervisor : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private ActorSelection? _iotSupervisor;

    public ApiSupervisor()
    {
        _log = Logging.GetLogger(Context);
        ReceiveAnyAsync(Test);
    }

    protected override void PreStart()
    {
        _log.Info($"{nameof(ApiSupervisor)} starting...");
        InstantiateSupervisors();
    }

    private void InstantiateSupervisors()
    {
        _log.Info($"{nameof(InstantiateSupervisors)} running...");

        _iotSupervisor = Context.ActorSelection("akka.tcp://IoTService@iot:5000/user/Supervisor");

        _log.Info($"{nameof(InstantiateSupervisors)} completed!");
    }

    private async Task Test(object message)
    {
        _log.Info($"{nameof(ApiSupervisor)} received message: {message}");

        // TODO: Use PipeTo, not await since this blocks any further action by
        // the actor
        var result = await _iotSupervisor.Ask(new TestMessage("Testing a message..."));

        _log.Info($"{nameof(ApiSupervisor)} received response: {result}");
        Sender.Tell(result);
    }
}