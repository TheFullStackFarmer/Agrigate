namespace Agrigate.IoT.Domain.DTOs;

/// <summary>
/// A telemetry reading from a device
/// </summary>
public class TelemetryReading
{
    /// <summary>
    /// The timestamp of the telemetry
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// The key for the telemetry
    /// </summary>
    public string Key { get; set; } = string.Empty;
    
    /// <summary>
    /// The value for the telemetry
    /// </summary>
    public double Value { get; set; }
}

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
    public List<TelemetryReading> Payload { get; set; } = [];
}