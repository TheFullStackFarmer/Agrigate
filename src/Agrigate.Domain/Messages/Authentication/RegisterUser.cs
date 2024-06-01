namespace Agrigate.Domain.Messages.Authentication;

/// <summary>
/// A message sent to the Authentication service to register a new user
/// </summary>
/// <param name="Username">The username to register</param>
/// <param name="Password">The password to register</param>
public record RegisterUser(string Username, string Password) : IAuthenticationMessage;