using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities;

namespace Agrigate.Authentication.Domain.Entities;

[Table(nameof(User))]
public class User : EntityBase
{
    [Key]
    public int Id { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool ForcePasswordReset { get; set; }
    public DateTimeOffset PasswordExpiration { get; set; }

    public DateTimeOffset? LastLogin { get; set; }

    public void ResetUserData(int passwordDuration)
    {
        var now = DateTimeOffset.UtcNow;

        FirstName = string.Empty;
        LastName = string.Empty;
        Phone = null;
        Email = null;
        ForcePasswordReset = false;
        PasswordExpiration = now.AddDays(passwordDuration);
        IsDeleted = false;
        Modified = now;
    }
}