using Agrigate.Domain.Messages;
using Agrigate.IoT.Actors.Devices;
using Agrigate.IoT.Domain.Contexts;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.IoT.Actors;

/// <summary>
/// The actor responsible for supervising the entire Agrigate IoT service
/// </summary>
public class IoTSupervisor : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private readonly IServiceProvider _serviceProvider;

    private IActorRef? _deviceManager;

    public IoTSupervisor(IServiceProvider serviceProvider)
    {
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        Receive<TestMessage>(Ping);
    }

    protected override void PreStart()
    {
        _log.Info($"{nameof(IoTSupervisor)} starting...");

        SetupDatabase();
        CreateManagers();

        _log.Info($"{nameof(IoTSupervisor)} ready!");
    }

    /// <summary>
    /// Ensures that the IoT service database is initialized and ready
    /// </summary>
    private void SetupDatabase()
    {
        _log.Info($"{nameof(SetupDatabase)} running...");

        using var scope = _serviceProvider.CreateScope();
        var dbFactory = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<IoTContext>>();
        using var db = dbFactory.CreateDbContext();

        db.Database.Migrate();

        _log.Info($"{nameof(SetupDatabase)} completed!");
    }

    /// <summary>
    /// Creates the required managers to run various aspects of the IoT service
    /// </summary>
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