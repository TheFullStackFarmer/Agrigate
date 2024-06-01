using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities;

namespace Agrigate.Authentication.Domain.Entities;

[Table(nameof(User))]
public class User : EntityBase
{
    [Key]
    public int Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool ForcePasswordReset { get; set; }
    public DateTimeOffset PasswordExpiration { get; set; }

    public DateTimeOffset LastLogin { get; set; }
}