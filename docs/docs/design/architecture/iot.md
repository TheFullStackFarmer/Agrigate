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
    broker["MQTT Broker"]:3

    supervisor --> manager
    manager --> device
    manager --> device2
    manager --> device3
    device --> broker
    device2 --> broker
    device3 --> broker
```
