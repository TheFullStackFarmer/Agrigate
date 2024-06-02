namespace Agrigate.Api.Features.Authentication.Models;

/// <summary>
/// DTO for registering a new user with Agrigate
/// </summary>
/// <param name="Username">An encrypted username</param>
/// <param name="Password">An encrypted password</param>
public record UserDTO(string Username, string Password);