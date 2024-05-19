# Services

The main business logic for Agrigate occurs in the service layer, which is made
up of a variety of microservices that are responsible for one aspect of
Agrigate, such as Authentication, IoT, Notifications, etc.

## Overview

Here, you can see which services interact with each other and the message broker
at a high level.

```mermaid
block-beta
    columns 2

    broker["MQTT Broker"]:2
    space:2
    iot(["IoT Service"]) auth(["Auth Service"])

    broker --> iot
    iot --> broker
```

## Service Components

Each service consists of the following components:

- A worker service, which performs the associated business logic
- A database (if required) to store essential information

Each service utilizes Akka.Remote in order to communicate with other services
when required and, in some situations, events will be published to the MQTT
broker in pre-specified channels. These channels can then be subscribed to by
other services, which will react accordingly.

```mermaid
block-beta
    columns 3

    broker(["MQTT Broker"]) database[("Database")] space:1
    space:3
    service(["Service"]) space:1 service2(["Service 2"])

    database --> service
    service -- "Events" --> broker
    service --> database
    service -- "Akka.Remote" --> service2
```
