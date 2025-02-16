# DBBroker Showcase Project

The easyest way of getting started with DBBroker is running the showcase using Docker Compose to run the supported databases.

**NOTE:** You will neet a Personal Access Token from <container-registry.oracle.com> and `docker login container-registry.oracle.com` to build the Oracle database image.

```bash
docker-compose build
docker-compose up -d
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
