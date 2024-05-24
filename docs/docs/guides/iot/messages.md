# Sending Device Messages

When connecting to the message broker with a device, the device must have a
unique DeviceId. Any messages sent by the device should be published to the
`/devices/{DeviceId}/messages` channel, while incoming messages from Agrigate
will be published to `/devices/{DeviceId}`.

After a device connects to the broker, Agrigate will send a message containing
a DeviceKey that is specific to the device that just connected. If messages
are published by the device without the DeviceKey, Agrigate will not act on
them.

An example message containing the DeviceKey will look like this:

```
{
    "Timestamp":"2024-05-24T11:38:08.7547185+00:00",
    "MessageId":"edd44828-868a-44c7-bf70-925f8aa2892e",
    "ReferenceId":null,
    "MessageType":1,
    "Method":null,
    "Payload":"82bc1960-20e6-4ca5-ba91-b33614befda9",
    "ExpectResponse":false
}
```

## Message Format

All messages sent to and from devices must be a json string that adheres to the
following format:

```
{
    Timestamp: DateTimeOffset,          # When the message was created
    MessageId: Guid,                    # Unique Id for the message
    ReferenceId: Guid?,                 # Original MessageId if this is a response to a prior message
    MessageType: DeviceMessageType,     # The type of message being sent
    Method: string?,                    # The method that should be invoked
    Payload: string?                    # The actual payload
    ExpectResponse: bool                # Whether a response is expected by the sending entity
}
```

where `DeviceMessageType` is an enum with the following values:

```
{
    Unknown = 0,
    DeviceKey = 1,      # The message contains a device key for future communications
    Telemetry = 2,      # The message contains telemetry data
    MethodCall = 3      # The message is meant to invoke a method
}
```
