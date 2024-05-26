using Agrigate.Domain.DTOs.IoT;

namespace Agrigate.IoT.Domain.Messages;

/// <summary>
/// A message for recording telemetry from a device
/// </summary>
/// <param name="DeviceId">The DeviceId that submitted the telemetry</param>
/// <param name="Telemetry">A list of telemetry to record</param>
public record TelemetryReceived(
    string DeviceId,
    List<TelemetryData> Telemetry
);