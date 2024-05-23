# Interacting with the IoT Service

The IoT service is a headless worker that manages the IoT aspects of Agrigate.
In order to connect to the service directly, you can utilize
[Petabridge.cmd](https://cmd.petabridge.com/).

## Installation

First, install dotnet and Petabridge.cmd with the following commands

```
# Windows
winget install Microsoft.DotNet.SDK.8
dotnet tool install -g pbm

# Linux
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
dotnet tool install -g pbm
```

Next, you can use the pbm command to connect to the IoT service

```
pbm {SERVER_IP}:5001
```
