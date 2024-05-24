namespace Agrigate.IoT.Domain.DTOs;

/// <summary>
/// An enum representing the type of message being sent
/// </summary>
public enum DeviceMessageType
{
    Unknown = 0,

    /// <summary>
    /// The message contains a device key that can be used for future 
    /// communications with Agrigate
    /// </summary>
    DeviceKey = 1,

    /// <summary>
    /// The message contains telemetry data
    /// </summary>
    Telemetry = 2,

    /// <summary>
    /// The message is meant to invoke a particular method
    /// </summary>
    MethodCall = 3,
}


/// <summary>
/// A message published to physical devices from
/// a Device Actor
/// </summary>
public class DeviceMessage
{
    /// <summary>
    /// The timestamp of when the message was created
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// A unique Id for the message to identify it and correlate any 
    /// responses received
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// A unique Id representing the original message's MessageId, if this 
    /// message is a response to a prior message
    /// </summary>
    public Guid? ReferenceId { get; set; }

    /// <summary>
    /// The type of message being sent
    /// </summary>
    public DeviceMessageType MessageType { get; set; }

    /// <summary>
    /// The method that should be invoked to handle the message
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// The payload that should be sent. Examples could be a regular string, 
    /// stringified number, or serialized object
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// Whether or not the sender of the message expects a response
    /// </summary>
    public bool ExpectResponse { get; set; }
}