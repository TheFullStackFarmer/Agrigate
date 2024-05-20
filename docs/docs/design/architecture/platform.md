# Platform

The Agrigate platform primarily consists of a web dashboard, web api, and
smaller microservices that make up the service layer.
[Akka.Net](https://petabridge.com/) is used to tie the various services and API
together via Akka.Remote, and services can be connected to manually through
[Petabridge.CMD](https://cmd.petabridge.com/).

There is also an MQTT broker that allows IoT devices to connect, and serves as
a broker for event-based messages between the various Agrigate services.

If configured, certain services will utilize 3rd party platforms, and any 3rd
party webhooks should be setup to connect with the appropriate API endpoints

```mermaid
block-beta
    columns 5

    broker(["MQTT Broker"]) space:1 services(["Service Layer"]) space:1 3rdParty(["Third Party Platforms"])
    space:5
    space:2 api(["Agrigate API"]) space:2
    space:5
    device(["Device 1"]) device2(["Device 2"]) dashboard(["Agrigate Dashboard"]) space:2
    space:5
    space:1 user(["User"]) space:3

    api --> services
    api --> dashboard
    broker --> device
    broker --> device2
    broker --> services
    dashboard --> api
    device --> broker
    device2 --> broker
    services --> broker
    services --> api
    services --> 3rdParty
    user --> device
    user --> device2
    user --> dashboard
    3rdParty --> api
```
