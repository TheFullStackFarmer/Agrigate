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

### Update Configuration

Coming soon!

### Run Docker-Compose

```
cd ./Agrigate
docker-compose build
docker-compose up
```
