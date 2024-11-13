# DBBroker

A lightweight and simple to use .NET library for manipulating database records.

## Quick Start ⚡

Install [DBBroker.Cli](https://www.nuget.org/packages/DBBroker.Cli) package to generate the Data Models.

```bash
dotnet install -g DBBroker.Cli
```

On the root of your .NET project initialize the `dbbroker.config.json`.

```bash
dbbroker init --namespace "EShop.DataModels" --connectionString "" --provider "SqlServer"
```

Generate/synchronize your Data Models.

```bash
dbbroker sync
```

Start using!

```C#
CustomersDataModel customer = new()
{
    Id = Guid.NewGuid(),
    Name = "John Three Sixteen",
    CreatedAt = DateTime.Now
};

using var connection = DbBroker.GetDbConnection<CustomersDataModel>();
connection.Insert(customer);
```

For more examples go [here](https://github.com/diegosiao/DBBroker).

## Contribute

Is there a Database provider you want to see supported? Contributors are welcome!

[![BuyMeACoffee](https://raw.githubusercontent.com/pachadotdev/buymeacoffee-badges/main/bmc-donate-yellow.svg)](https://www.buymeacoffee.com/diegosiao)
