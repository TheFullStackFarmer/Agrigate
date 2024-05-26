namespace Agrigate.Domain.DTOs.IoT;

/// <summary>
/// Representation of device telemetry 
/// </summary>
public class TelemetryData
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
/// Telemetry data for a given device
/// </summary>
public class DeviceTelemetry
{
    /// <summary>
    /// The DeviceId of the device
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Telemetry for the device
    /// </summary>
    public List<TelemetryData> Telemetry { get; set; } = [];
}