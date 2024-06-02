using Agrigate.Domain.Messages.Authentication;
using Akka.Actor;
using Akka.DependencyInjection;

namespace Agrigate.Authentication.Actors.Users;

/// <summary>
/// The actor responsible for managing Agrigate users
/// </summary>
public class UserManager : ReceiveActor
{
    public UserManager()
    {
        Receive<RegisterUser>(HandleUserRegistration);
        Receive<GenerateToken>(HandleTokenGeneration);
    }

    private void HandleUserRegistration(RegisterUser message)
    {
        AskFor(message);
    }

    private void HandleTokenGeneration(GenerateToken message)
    {
        AskFor(message);
    }

    // TODO: Move this to an AgrigateActor in Agrigate.Core, then update
    // MQTTActor
    protected static void AskFor<TMessage>(TMessage message)
        where TMessage : class
    {
        var queryProps = DependencyResolver.For(Context.System).Props<UserQueryActor>();
        var queryHandler = Context.ActorOf(queryProps);

        queryHandler.Forward(message);
    }
}