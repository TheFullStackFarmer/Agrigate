using Agrigate.Core;
using Agrigate.Domain.Messages.IoT;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.Api.Core;

/// <summary>
/// The actor responsible for supervising the Agrigate API actors
/// </summary>
public class ApiSupervisor : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private readonly ApiConfiguration _configuration;

    private ActorSelection? _iotSupervisor;

    public ApiSupervisor(ApiConfiguration config)
    {
        _log = Logging.GetLogger(Context);
        _configuration = config ?? throw new ArgumentNullException(nameof(config));

        ReceiveAny(RequestHandler);
    }

    protected override void PreStart()
    {
        _log.Info($"{nameof(ApiSupervisor)} starting...");
        InstantiateSupervisors();
    }

    private void InstantiateSupervisors()
    {
        _log.Info($"{nameof(InstantiateSupervisors)} running...");

        var iotConfig = _configuration.IoTService;
        _iotSupervisor = Context.ActorSelection($"akka.tcp://{iotConfig.ServiceName}@{iotConfig.Hostname}:{iotConfig.Port}/user/Supervisor");

        _log.Info($"{nameof(InstantiateSupervisors)} completed!");
    }

    private void RequestHandler(object message)
    {
        try
        {
            switch (message)
            {
                case IIoTMessage:
                    _iotSupervisor.Ask(
                        message, 
                        Constants.MaxActorWaitTime
                    ).PipeTo(Sender);
                    break;
                default:
                    var messageType = message.GetType().Name;
                    _log.Warning($"Unhandled message of type {messageType}");
                    Sender.Tell(new ApplicationException("Unhandled Message Type"));
                    break;
            }
        }
        catch (Exception ex)
        {
            var error = $"Error making request: {ex.Message}";
            _log.Error(error);
            Sender.Tell(new ApplicationException(error));
        }
    }
}