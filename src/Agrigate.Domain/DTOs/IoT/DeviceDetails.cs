namespace Agrigate.Domain.DTOs.IoT;

/// <summary>
/// Details about a device's method
/// </summary>
public class MethodDetails
{
    /// <summary>
    /// The name of the method
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// A description of the method
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// A collection of parameters and types that the method expects
    /// </summary>
    public Dictionary<string, string>? Parameters { get; set; }
}

/// <summary>
/// Details about a specific device
/// </summary>
public class DeviceDetails
{
    /// <summary>
    /// The device's DeviceId
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// The last time the device connected
    /// </summary>
    public DateTimeOffset LastConnection { get; set; }

    /// <summary>
    /// The model of the device
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// The serial number of the device
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Whether the device is currently online
    /// </summary>
    public bool Online { get; set; }

    /// <summary>
    /// A list of methods that are exposed on the device
    /// </summary>
    public List<MethodDetails>? Methods { get; set; }
}