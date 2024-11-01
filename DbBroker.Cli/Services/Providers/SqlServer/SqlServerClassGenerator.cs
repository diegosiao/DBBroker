using System.Text;
using DbBroker.Cli.Extensions;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerClassGenerator : IClassGenerator
{
    public async Task<int> GenerateAsync(DbBrokerConfigContext context)
    {
        "Retrieving database metadata...".Log();
        
        using var connection = context.GetDbConnection();

        var tableDescriptors = await context
            .GetMetadataProvider()
            .GetTableDescriptorsAsync(connection, context);

        foreach (var tableDescriptor in tableDescriptors)
        {
            var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "eShop", "DataModels");
            Directory.CreateDirectory(outputDirectory);
            
            var propsString = new StringBuilder();
            foreach (var item in tableDescriptor.Value.Columns)
            {
                var isPrimaryKey = tableDescriptor.Value.Keys.Any(x => x.KeyFullName.Equals(item.ColumnFullName) && x.ConstraintType.Equals("PrimaryKey"));

                propsString.AppendLine(
                    Constants.EDM_PROPERTY_TEMPLATE
                        .Replace("$TYPE", new SqlServerSqlTransformer().GetCSharpType(item.DataType, item?.MaxLength ?? "50", item?.IsNullable ?? false))
                        .Replace("$KEY", isPrimaryKey ? "Key, " : string.Empty)
                        .Replace("$COLUMN_NAME", item?.ColumnName)
                        .Replace("$NAME", item?.ColumnName.ToCamelCase()));
            }

            File.WriteAllText(
                Path.Combine(outputDirectory, $"{tableDescriptor.Value.TableName}Edm.cs"),
                Constants.EDM_CLASS_TEMPLATE
                    .Replace("$NAMESPACE", context.Namespace ?? "-")
                    .Replace("$CLASSNAME", tableDescriptor.Value.TableName)
                    .Replace("$PROPERTIES", propsString.ToString()));
        }

        return 0;
    }
}
