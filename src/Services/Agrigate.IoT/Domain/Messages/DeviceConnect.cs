namespace Agrigate.IoT.Domain.Messages;

/// <summary>
/// A message for handling logic when a device has connected
/// </summary>
/// <param name="DeviceId">The id of the device that has connected</param>
public record DeviceConnect(string DeviceId);