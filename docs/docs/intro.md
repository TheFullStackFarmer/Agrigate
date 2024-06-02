---
slug: /
sidebar_position: 1
---

# Introduction

Agrigate is a platform that collects, manages, and analyzes all of your
agricultural data to help you run a more efficient, profitable farm. As long as
you have a computer and internet connection, you can download and install
agrigate in as little as 5 minutes.

Once installed, no internet connection is needed; however, a local network is
required to access the application. In addtion, some features will not function
properly without an active internet connection.

## Technical Details

Agrigate is built using Docker, .Net and React. It also uses certain third-party
platforms.

### Dependencies

#### Infrastructure

- [Docker](https://www.docker.com/) - Orchestration
- [EMQX](https://www.emqx.io/) - MQTT Broker
- [Postgres](https://www.postgresql.org/) - Database

#### Services & API

- [.Net](https://dotnet.microsoft.com/en-us/) - Business logic and services
- [Akka.Net](https://petabridge.com/) - Actor Model
- [MQTTnet](https://github.com/dotnet/MQTTnet) - MQTT Communication
- [EF Core](https://learn.microsoft.com/en-us/ef/core/) - Data Model & Migrations

#### Web Application

- [React](https://react.dev/) - UI
- [JoyUI](https://mui.com/joy-ui/getting-started/) - UI Components
- [FontAwesome](https://fontawesome.com/) - Icons
- [Day.js](https://day.js.org/) - Date & Time calculations
- [Recharts](https://recharts.org/en-US/) - Charts
- [Recoil](https://recoiljs.org/) - State management

### Versioning

Agrigate uses [semantic versioning](https://semver.org/) following the
\{MAJOR\}.\{MINOR\}.\{PATCH\} format

Release notes for each version can be found via the Releases page

## Contact Information

Agrigate is developed and maintained by
[The Full Stack Farmer](https://thefullstackfarmer.com/). For assistance, reach
out to [support@thefullstackfarmer.com](mailto:support@thefullstackfarmer.com)
