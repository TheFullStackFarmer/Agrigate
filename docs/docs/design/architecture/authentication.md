# Authentication Service

The Authentication service is responsible for managing users and JWTs for the
Agrigate API.

## Overview

A single supervisor is at the root of the Authentication service, which
oversees a User Manager. The User Manager is responsible for handling all
incoming user requests, which are forwarded to a short-lived actor.

```mermaid
block-beta
    columns 1

    supervisor(("Supervisor"))
    space:1
    manager(("User Manager"))

    supervisor --> manager
```

## Data Model

The Authentication service utilizes the following data model.

- **User**: A user of the Agrigate platform

```mermaid
erDiagram

    User {
        int Id
        string FirstName
        string LastName
        string Phone
        string Email
        string Username
        string Password
        bool ForcePasswordReset
        DateTimeOffset PasswordExpiration
        DateTimeOffset LastLogin
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
