namespace Agrigate.Domain.Messages.IoT;

/// <summary>
/// A message sent to the IoT service to retrieve telemetry for a particular 
/// device
/// </summary>
/// <param name="DeviceId"></param>
/// <param name="StartTime"></param>
/// <param name="EndTime"></param>
public record GetTelemtry(
    string DeviceId,
    DateTimeOffset? StartTime,
    DateTimeOffset? EndTime
) : IDeviceMessage;