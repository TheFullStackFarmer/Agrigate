namespace Agrigate.Domain.DTOs.Authentication;

/// <summary>
/// An object containing the JWT for an authorized user
/// </summary>
public class UserToken
{
    public string Token { get; set; } = string.Empty;
}