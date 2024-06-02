using Agrigate.Core.Actors;
using Agrigate.Domain.Messages.Authentication;

namespace Agrigate.Authentication.Actors.Users;

/// <summary>
/// The actor responsible for managing Agrigate users
/// </summary>
public class UserManager : AgrigateActor
{
    public UserManager()
    {
        Receive<RegisterUser>(HandleUserRegistration);
        Receive<GenerateToken>(HandleTokenGeneration);
    }

    private void HandleUserRegistration(RegisterUser message)
    {
        AskFor<RegisterUser, UserQueryActor>(message);
    }

    private void HandleTokenGeneration(GenerateToken message)
    {
        AskFor<GenerateToken, UserQueryActor>(message);
    }
}