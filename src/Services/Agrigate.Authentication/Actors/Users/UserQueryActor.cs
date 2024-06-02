using Agrigate.Authentication.Domain.Contexts;
using Agrigate.Authentication.Domain.Entities;
using Agrigate.Authentication.Services;
using Agrigate.Core.Utilities;
using Agrigate.Domain.DTOs;
using Agrigate.Domain.DTOs.Authentication;
using Agrigate.Domain.Messages.Authentication;
using Akka.Actor;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Authentication.Actors.Users;

/// <summary>
/// An actor responsible for interacting with the Authentication Service database 
/// for User-related requests
/// </summary>
public class UserQueryActor : ReceiveActor
{
    private readonly int _validPasswordDuration;
    private readonly IDbContextFactory<AuthenticationContext> _dbFactory;
    private readonly IAuthenticationService _authenticationService;

    public UserQueryActor(
        IDbContextFactory<AuthenticationContext> dbFactory,
        IAuthenticationService authenticationService
    )
    {
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        _authenticationService = authenticationService 
            ?? throw new ArgumentNullException(nameof(authenticationService));

        _validPasswordDuration = 90;

        Receive<RegisterUser>(HandleUserRegistration);
        Receive<GenerateToken>(HandleTokenGeneration);
    }

    /// <summary>
    /// Attempts to register a new user for Agrigate
    /// </summary>
    /// <param name="message">A message containing the user information that needs to
    /// be registered</param>
    private void HandleUserRegistration(RegisterUser message)
    {
        if (string.IsNullOrEmpty(message.Username) || string.IsNullOrEmpty(message.Password))
        {
            Sender.Tell(new ValidationError("Username and Password are required"));
            Self.Tell(PoisonPill.Instance);
            return;
        }

        User? result = null;
        var timestamp = DateTimeOffset.UtcNow;
        using var db = _dbFactory.CreateDbContext();

        var existingUser = db.Users.FirstOrDefault(u => u.Username == message.Username);

        if (existingUser != null && !existingUser.IsDeleted)
        {
            Sender.Tell(new ValidationError("User already exists"));
            Self.Tell(PoisonPill.Instance);
            return;
        }

        else if (existingUser != null && existingUser.IsDeleted)
        {
            var newPassword = HashUtility.Hash(message.Password);
            existingUser.Password = newPassword;
            existingUser.ResetUserData(_validPasswordDuration);

            result = existingUser;
        }

        else 
        {
            var newPassword = HashUtility.Hash(message.Password);
            result = new User 
            {
                Username = message.Username,
                Password = newPassword,
                PasswordExpiration = timestamp.AddDays(_validPasswordDuration),
                Created = timestamp,
                Modified = timestamp
            };

            db.Add(result);
        }

        db.SaveChanges();
        
        Sender.Tell(new UserRegistration 
        {
            Username = result?.Username ?? string.Empty,
            Created = result?.Created ?? timestamp
        });
        
        Self.Tell(PoisonPill.Instance);
    }

    /// <summary>
    /// Attempts to generate a token for the specified user
    /// </summary>
    /// <param name="message">A message containing the provided login information for
    /// a user</param>
    private void HandleTokenGeneration(GenerateToken message)
    {
        if (string.IsNullOrEmpty(message.Username) || string.IsNullOrEmpty(message.Password))
        {
            Sender.Tell(new Unauthorized());
            Self.Tell(PoisonPill.Instance);
            return;
        }

        var timestamp = DateTimeOffset.UtcNow;
        using var db = _dbFactory.CreateDbContext();

        var existingUser = db.Users.FirstOrDefault(u => 
            u.Username == message.Username
            && !u.IsDeleted
        );

        if (existingUser == null)
        {
            Sender.Tell(new Unauthorized());
            Self.Tell(PoisonPill.Instance);
            return;
        }

        if (!HashUtility.Verify(message.Password, existingUser!.Password))
        {
            Sender.Tell(new Unauthorized());
            Self.Tell(PoisonPill.Instance);
            return;
        }

        var token = _authenticationService
            .GenerateToken(existingUser!);
        
        existingUser!.LastLogin = timestamp;

        Sender.Tell(new UserToken { Token = token });
        Self.Tell(PoisonPill.Instance);
    }
}