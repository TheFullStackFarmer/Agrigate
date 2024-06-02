using Agrigate.Core.Configuration;

namespace Agrigate.Api.Core;

/// <summary>
/// Configuration settings for the API
/// </summary>
public class ApiConfiguration
{
    /// <summary>
    /// The configuration for the Api
    /// </summary>
    public ServiceConfiguration Service { get; set; } = new();

    /// <summary>
    /// The configuration for the IoT service
    /// </summary>
    public ServiceConfiguration IoTService { get; set; } = new();

    /// <summary>
    /// The configuration for the Authentication service
    /// </summary>
    public ServiceConfiguration AuthenticationService { get; set; } = new();

    /// <summary>
    /// Authentication Options for the API
    /// </summary>
    public AuthenticationOptions Authentication { get; set; } = new();
}