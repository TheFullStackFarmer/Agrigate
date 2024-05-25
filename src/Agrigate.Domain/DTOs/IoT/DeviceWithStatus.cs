namespace Agrigate.Domain.DTOs.IoT;

/// <summary>
/// A minimal representation of a device with it's current online status
/// </summary>
public class DeviceWithStatus
{
    /// <summary>
    /// The DeviceId for the device
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// The last time the device connected
    /// </summary>
    public DateTimeOffset LastConnection { get; set; }

    /// <summary>
    /// Whether the device is currently online
    /// </summary>
    public bool Online { get; set; }
}