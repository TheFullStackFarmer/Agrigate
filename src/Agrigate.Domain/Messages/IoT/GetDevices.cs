namespace Agrigate.Domain.Messages.IoT;

/// <summary>
/// A message sent to the IoT service to retrieve a list of devices
/// </summary>
public record GetDevices() : IDeviceMessage;