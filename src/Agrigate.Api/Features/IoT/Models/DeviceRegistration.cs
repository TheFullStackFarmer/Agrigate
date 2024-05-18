namespace Agrigate.Api.Controllers.Features.IoT.Models;

/// <summary>
/// DTO for registering a new device with Agrigate
/// </summary>
/// <param name="DeviceId">A name or id for the device</param>
/// <param name="Model">An optional model</param>
/// <param name="SerialNumber">An optional serial number</param>
public record DeviceRegistration(
    string DeviceId,
    string? Model,
    string? SerialNumber
);