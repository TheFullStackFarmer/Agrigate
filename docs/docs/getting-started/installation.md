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
- dc                          # Docker-Compose volumes
  - message-broker
    - etc                     # EMQX config files
  - database
    - data                    # Postgres data files
- src
  - Tests
    - Agrigate.xxx.Tests      # Test projects the API and each service
  - Services
    - Agrigate.Authentication # The Authentication service
    - Agrigate.IoT            # The IoT service
  - Agrigate.Api              # The public api for interacting with Agrigate
  - Agrigate.Domain           # Models and data shared between projects
- docs                        # This documentation site
```

## Setup

To start running Agrigate, complete the following steps:

### Clone the Repo

```
git clone https://github.com/TheFullStackFarmer/Agrigate.git
```

### Configuration

#### Defaults

Agrigate is configured to use the following ports by default:

##### MQTT

| Item             | Port  | Exposed Docker Port |
| ---------------- | ----- | ------------------- |
| MQTT             | 1883  | 1883                |
| MQTT / SSL       | 8883  | 8883                |
| MQTT / Websocket | 8083  | 8083                |
| Dashboard        | 18083 | 18083               |

##### Database

| Item       | Port | Exposed Docker Port |
| ---------- | ---- | ------------------- |
| PostgreSQL | 5432 | 8000                |

##### API

| Item            | Port | Exposed Docker Port |
| --------------- | ---- | ------------------- |
| Api Akka.Remote | 8081 |                     |
| HTTP Web API    | 8080 | 8080                |

##### Services

| Item                          | Port | Exposed Docker Port |
| ----------------------------- | ---- | ------------------- |
| IoT Akka.Remote               | 5000 |                     |
| IoT Petabridge.CMD            | 5001 | 5001                |
| Authentication Akka.Remote    | 5000 |                     |
| Authentication Petabridge.CMD | 5001 | 5011                |

##### Portal

| Item       | Port | Exposed Docker Port |
| ---------- | ---- | ------------------- |
| Web Portal | 3000 | 8081                |

These can be changed to whatever you want via the `docker-compose.yml` and
`DockerFile.{Service}` files.

#### Updates

Before running Agrigate, it's recommended to change the following configurations
and environment variables

##### Portal

For the web portal, update the following in `src/Web/portal/.env`

```
NEXTAUTH_SECRET=XXX
```

### Run Docker-Compose

```
cd ./Agrigate
docker-compose build
docker-compose up
```

At this point, you should the following containers running:

- EMQX
- Postgres
- IoT service
- Authentication service
- API
- Web Portal

You can access the MQTT broker by navigating to http://localhost:18083 and
logging in with:

- Username: admin
- Password: public

You'll be promted to change the password when you log in

You can also access the API swagger docs at http://localhost:8080/swagger

### Create a User

In order to log in and utilize Agrigate, you must first create a user. The
initial user will need to be created via the API at
`http://localhost:8080/Authentication/Register`. You can either use an API
client of your choice or the swagger docs. The payload will look like

```
{
  "Username": "MyUser",
  "Password": "MyPassword"
}
```

Once created, those crentials can be used to login to the web app at
http://localhost:8081
