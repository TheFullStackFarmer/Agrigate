using Agrigate.Domain.DTOs.IoT;

namespace Agrigate.IoT.Domain.DTOs;

/// <summary>
/// A telemetry message sent from a device to Agrigate
/// </summary>
public class DeviceTelemetry
{
    /// <summary>
    /// The DeviceKey associated with the Device submitting telemetry
    /// </summary>
    public string DeviceKey { get; set; } = string.Empty;
    
    /// <summary>
    /// The timestamp when the message was sent
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// A collection of telemetry readings
    /// </summary>
    public List<TelemetryData> Payload { get; set; } = [];
}