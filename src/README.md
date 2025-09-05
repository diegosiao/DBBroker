# DBBroker Tests or Showcase

The DBBroker Tests and Showcase CLI application share the same databases.

## Prerequisites

- Docker;
- .NET 8 SDK or later;

## Get Started

Before running the Tests or Showcase CLI application, make sure you spin up the databases running:

```bash
docker compose --profile all --file ./Databases/docker-compose.yaml up --detach
```

**IMPORTANT**: The Oracle database container image is not on Docker Hub, so you might need to `docker login` into [Oracle Registry](https://container-registry.oracle.com/), or use `no-oracle` profile instead of `all`.

To stop and clean up the containers:

```bash
docker compose --profile all --file ./Databases/docker-compose.yaml down --volumes
```
