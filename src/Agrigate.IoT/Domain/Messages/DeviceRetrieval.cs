namespace Agrigate.IoT.Domain.Messages;

/// <summary>
/// A message for retrieving devices that have connected to Agrigate and their
/// current status
/// </summary>
/// <param name="ActiveDevices">The currently connected devices</param>
public record DeviceRetrieval(List<string> ActiveDevices, string? DeviceId);