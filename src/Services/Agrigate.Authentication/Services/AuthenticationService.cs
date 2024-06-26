using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Agrigate.Authentication.Domain.Entities;
using Agrigate.Core.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Agrigate.Authentication.Services;

/// <inheritdoc />
public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationOptions _options;

    public AuthenticationService(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", $"{user.FirstName} {user.LastName}"),
            new("givenname", user.FirstName ?? ""),
            new("surname", user.LastName ?? ""),
            new("emailaddress", user.Email ?? ""),
            new("homephone", user.Phone ?? ""),
            new("upn", user.Username)
        };

        var tokenOptions = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(_options.TokenDurationHours),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var tokenString = handler.WriteToken(tokenOptions);

        return tokenString;
    }
}