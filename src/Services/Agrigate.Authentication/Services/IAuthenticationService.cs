using Agrigate.Authentication.Domain.Entities;

namespace Agrigate.Authentication.Services;

/// <summary>
/// A service for handling authentication-specific logic
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Generates a JWT for the provided user
    /// </summary>
    /// <param name="user">The user for which to generate a JWT</param>
    /// <returns></returns>
    public string GenerateToken(User user);
}