# IoT Service

The IoT service is responsible for communicating and interacting with various
devices that connect to the Agrigate platform.

## Overview

A single supervisor is at the root of the IoT Service, which oversees the Device
Manager. The Device Manager is responsible for monitoring connect and disconnect
events coming from the IoT broker.

On every connect or disconnect event, the Device manager will create or destroy
a device actor for the physical device.

The device actors are responsible for communicating with their associated
physical devices, and maintain a separate connection to the MQTT broker.

```mermaid
block-beta
    columns 3

    space:1 supervisor(("Supervisor")) space:1
    space:1 manager(("Device Manager")) space:1
    device(("Device 1")) device2(("Device 2")) device3(("Device 3"))
    broker(["MQTT Broker"]):3
    pdevice(["Physical Device 1"]) pdevice2(["Physical Device 2"]) pdevice3(["Physical Device 3"])

    supervisor --> manager
    manager --> device
    manager --> device2
    manager --> device3
    device --> broker
    device2 --> broker
    device3 --> broker
    broker --> device
    broker --> device2
    broker --> device3
    pdevice --> broker
    pdevice2 --> broker
    pdevice3 --> broker
    broker --> pdevice
    broker --> pdevice2
    broker --> pdevice3
```

## Data Model

The IoT service utilizes the following data model

- **Device**: A physical device that has connected to the Agrigate platform
- **DeviceMethod**: A method that is exposed on the device and can be initiated
  remotely by Agrigate
- **Telemetry**: Sensor or other data received from the device

```mermaid
erDiagram
    Device ||--o{ DeviceMethod : Contains
    Device ||--o{ Telemetry : Records
    Device {
        int Id
        Guid DeviceKey
        string DeviceId
        string Model
        string SerialNumber
        DateTimeOffset LastConnection
    }
    DeviceMethod {
        int Id
        int DeviceId
        string Name
        string Description
        string Parameters
    }
    Telemetry {
        long Id
        int DeviceId
        DateTimeOffset Timestamp
        string Key
        double Value
        bool BoolValue
        string StringValue
    }
    Log {
        long Id
        DateTimeOffset Timestamp
        int LogLevel
        string Message
        string Source
        string Data
        string StackTrace
    }
```
