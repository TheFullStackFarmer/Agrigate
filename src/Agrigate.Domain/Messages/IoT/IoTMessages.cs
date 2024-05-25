namespace Agrigate.Domain.Messages.IoT;

/// <summary>
/// A message meant for the IoT service
/// </summary>
public interface IIoTMessage 
{
}

/// <summary>
/// A message intended for the Device domain of the IoT service
/// </summary>
public interface IDeviceMessage : IIoTMessage
{
}