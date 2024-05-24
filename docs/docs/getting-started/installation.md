---
sidebar_position: 1
---

# Installation

Agrigate consists of several components and can be installed via docker-compose.

## Requirements

Before installing Agrigate, be sure to have the following:

- [Git](https://git-scm.com/downloads)
- [Docker](https://docs.docker.com/desktop/install/linux-install/) &
  [Docker Compose](https://docs.docker.com/compose/install/)

## Project Structure

```
- README.md
- docker-compose.yml
- dc
  - message-broker
    - etc               # EMQX config files
- src
  - Agrigate.Api        # The public api for interacting with Agrigate
  - Agrigate.Domain     # Models and data shared between projects
  - Agrigate.IoT        # The IoT service
- docs                  # This documentation site
```

## Setup

To start running Agrigate, complete the following steps:

### Clone the Repo

```
git clone https://github.com/TheFullStackFarmer/Agrigate.git
```

### Configuration

Agrigate is configured to use the following ports by default:

#### MQTT

| Item             | Port  | Exposed Docker Port |
| ---------------- | ----- | ------------------- |
| MQTT             | 1883  | 1883                |
| MQTT / SSL       | 8883  | 8883                |
| MQTT / Websocket | 8083  | 8083                |
| Dashboard        | 18083 | 18083               |

#### Database

| Item       | Port | Exposed Docker Port |
| ---------- | ---- | ------------------- |
| PostgreSQL | 5432 | 8000                |

#### API

| Item            | Port | Exposed Docker Port |
| --------------- | ---- | ------------------- |
| Api Akka.Remote | 8081 |                     |
| HTTP Web API    | 8080 | 8080                |

#### Services

| Item               | Port | Exposed Docker Port |
| ------------------ | ---- | ------------------- |
| IoT Akka.Remote    | 5000 |                     |
| IoT Petabridge.CMD | 5001 | 5001                |

These can be changed to whatever you want via the `docker-compose.yml` and
`DockerFile.{Service}` files.

### Run Docker-Compose

```
cd ./Agrigate
docker-compose build
docker-compose up
```

At this point, you should see an MQTT broker, the IoT service, and API running.
You can access the MQTT broker by navigating to http://localhost:18083 and
logging in with:

- Username: admin
- Password: public

You'll be promted to change the password when you log in
