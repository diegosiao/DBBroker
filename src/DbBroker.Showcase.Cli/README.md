# DBBroker Showcase Project

The easyest way of getting started with DBBroker is running the showcase using Docker Compose to run the supported databases.

The Database vendors versions are:

- Oracle 21.3.0 XE
- SQL Server 2019 - latest

Start the databases by running:

```bash
docker compose build
docker compose up -d
```

You can also build and run a single database selectively, if you prefer.

```bash
docker-compose build sqlserver
docker-compose up -d sqlserver
```

To install DBBroker CLI .NET tool, run:

```bash
dotnet tool install --global DBBroker.Cli
```
