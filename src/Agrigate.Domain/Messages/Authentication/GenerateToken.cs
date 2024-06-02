namespace Agrigate.Domain.Messages.Authentication;

/// <summary>
/// A message sent to the Authentication service to generate a JWT token for
/// the user
/// </summary>
/// <param name="Username">The username to register</param>
/// <param name="Password">The password to register</param>
public record GenerateToken(string Username, string Password) : IAuthenticationMessage;