namespace Agrigate.Api.Features.Authentication.Models;

/// <summary>
/// A container for a public and private key pair
/// </summary>
/// <param name="PublicKey">The public RSA key</param>
/// <param name="PrivateKey">The private RSA key</param>
public record RsaKeyPair(string PublicKey, string PrivateKey);