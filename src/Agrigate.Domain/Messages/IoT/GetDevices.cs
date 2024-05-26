namespace Agrigate.Domain.Messages.IoT;

/// <summary>
/// A message sent to the IoT service to retrieve a list of devices
/// </summary>
/// <param name="DeviceId">A specific DeviceId to retrieve information on</param>
public record GetDevices(string? DeviceId = null) : IDeviceMessage;