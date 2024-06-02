namespace Agrigate.Domain.DTOs.Authentication;

/// <summary>
/// A object containing the newly registered user's username and created timestamp
/// </summary>
public class UserRegistration
{
    public string Username { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
}