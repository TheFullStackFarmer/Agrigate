using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Authentication.Domain.Entities;

/// <summary>
/// Represents a user of the Agrigate platform
/// </summmary>
[Table(nameof(User))]
[Index(nameof(Username), IsUnique = true)]
public class User : EntityBase
{
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// The user's first name
    /// </summmary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The user's last name
    /// </summmary>
    public string? LastName { get; set; }

    /// <summary>
    /// The user's phone number
    /// </summmary>
    public string? Phone { get; set; }

    /// <summary>
    /// The user's email address
    /// </summmary>
    public string? Email { get; set; }

    /// <summary>
    /// A unique username for the user
    /// </summmary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The hashed password for the user
    /// </summmary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether a password reset is required on the next login
    /// </summmary>
    public bool ForcePasswordReset { get; set; }

    /// <summary>
    /// The datetime at which the password will expire
    /// </summmary>
    public DateTimeOffset PasswordExpiration { get; set; }

    /// <summary>
    /// The last time the user logged in
    /// </summmary>
    public DateTimeOffset? LastLogin { get; set; }

    /// <summary>
    /// Resets the user's data to the initial state
    /// </summmary>
    /// <param name="passwordDuration">The duration of the password</param  
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