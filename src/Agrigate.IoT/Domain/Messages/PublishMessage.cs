using Agrigate.IoT.Domain.DTOs;

namespace Agrigate.IoT.Domain.Messages;

/// <summary>
/// A message for handling a request to publish a message to the message broker
/// </summary>
/// <param name="Topic">The topic where the message should be published</param>
/// <param name="Payload">The payload of the message</param>
public record PublishMessage(string Topic, DeviceMessage Payload);