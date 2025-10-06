# DBBroker CLI

Generate your Data Models easily and work seamlessly with [DBBroker](https://www.nuget.org/packages/DBBroker) Micro ORM Library.

```bash
dotnet tool install --global DbBroker.Cli

# Or if you are building from source
dotnet tool install --global --prerelease --add-source ./bin/Debug/ dbbroker.cli
```

On your terminal navigate to the `.csproj` file of the DataModels project directory then follow the instructions below.

## `dbbroker init`

Initializes the `dbbroker.config.json` file using the values specified.

```bash
dbbroker init --namespace "MyApp.DataModels" --connectionsString "<my_connection_string>" --provider "SqlServer"
```

## `dbbroker sync`

Synchronizes the Data Models with the databases specified in one or more `dbbroker.config.json` files.

```bash
dbbroker sync
```

## Contribute

We appreciate all contributions, whether they're bug reports, feature suggestions, or pull requests. Thank you for your interest and support in improving this project!

Financial support is also welcome, whether large or small contributions will help to keep this project moving and always secure.

[![BuyMeACoffee](https://raw.githubusercontent.com/pachadotdev/buymeacoffee-badges/main/bmc-donate-yellow.svg)](https://www.buymeacoffee.com/diegosiao)
