namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration options for the Authentication service used for 
/// generating JWTs
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// The issuer of the JWT
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The audience for the JWT
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// A secret key used to sign the JWT
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The number of hours a generated token is valid for
    /// </summary>
    public int TokenDurationHours { get; set; }
}