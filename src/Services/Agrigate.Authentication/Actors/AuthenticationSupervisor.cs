using Agrigate.Authentication.Domain.Contexts;
using Agrigate.Domain.Messages.Authentication;
using Akka.Actor;
using Akka.Event;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Authentication.Actors;

public class AuthenticationSupervisor : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private readonly IServiceProvider _serviceProvider;

    public AuthenticationSupervisor(IServiceProvider serviceProvider)
    {
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        ReceiveAny(RequestHandler);
    }

    protected override void PreStart()
    {
        _log.Info($"{nameof(AuthenticationSupervisor)} starting...");

        SetupDatabase();

        _log.Info($"{nameof(AuthenticationSupervisor)} ready!");
    }

    /// <summary>
    /// Ensures that the IoT service database is initialized and ready
    /// </summary>
    private void SetupDatabase()
    {
        _log.Info($"{nameof(SetupDatabase)} running...");

        using var scope = _serviceProvider.CreateScope();
        var dbFactory = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<AuthenticationContext>>();
        using var db = dbFactory.CreateDbContext();

        db.Database.Migrate();

        _log.Info($"{nameof(SetupDatabase)} completed!");
    }

    private void RequestHandler(object message)
    {
        switch (message)
        {
            case IAuthenticationMessage:
                Sender.Tell(true);
                break;
                
            default:
                var messageType = message.GetType().Name;
                _log.Warning($"Unhandled message of type {messageType}");
                Sender.Tell(new ApplicationException("Unhandled Message Type"));
                break;
        }
    }
}