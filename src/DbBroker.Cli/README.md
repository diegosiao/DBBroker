# DBBroker CLI

Generate your Data Models easily and work seamelessly with [DBBroker](https://www.nuget.org/packages/DBBroker) Micro ORM Library.

```bash
dotnet tool install --global DbBroker.Cli
# Or locally
dotnet tool install --global --add-source ./bin/Debug/ dbbroker.cli
```

## `dbbroker init`

Initializes the `dbbroker.config.json` file using the values specified.

```bash
dbbroker init
```

## `dbbroker sync`

Synchronizes the Data Models with the databases specified in one or more `dbbroker.config.json` files.

```bash
dbbroker sync
```
