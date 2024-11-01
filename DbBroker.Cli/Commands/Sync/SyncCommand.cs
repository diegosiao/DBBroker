using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using Dapper;
using DbBroker.Cli.Model;

namespace DbBroker.Cli.Commands.Sync;

public class SyncCommand
{
    static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static int Execute(SyncOptions options)
    {
        "Synchronizing...".Log();
        var configFilesDirectory = options.ConfigFilesDirectory ?? Directory.GetCurrentDirectory();
        var configFiles = Directory.EnumerateFiles(configFilesDirectory, "dbbroker.config.*");

        if (!configFiles.Any())
        {
            $"No configuration file was found at '{configFilesDirectory}'.".Error();
            $"Run 'dbbroker init' at your *.csproj file level to create one.".Error();
            return ExitCodes.CONFIG_FILE_NOTFOUND;
        }

        $"Loading {configFiles.Count()} configuration(s) file(s):".Log();
        List<DbBrokerConfig> configs = [];
        foreach (var configFile in configFiles)
        {
            $"- {configFile}".Log();
            if (TryParseConfig(configFile, out DbBrokerConfig? config))
            {
                configs.Add(config!);
            }
        }

        using (var connection = new SqlConnection(configs[0]?.Databases?.First().ConnectionString))
        {
            connection.Open();

            var columns = connection.Query<DbBrokerConfigDatabaseColumn>(
            @"
            SELECT
                s.name AS SchemaName,
                t.name AS TableName,
                c.name AS ColumnName,
                ty.name AS DataType,
                c.max_length AS MaxLength,
                c.is_nullable AS IsNullable
            FROM
                sys.tables AS t
            INNER JOIN
                sys.schemas AS s ON t.schema_id = s.schema_id
            INNER JOIN
                sys.columns AS c ON t.object_id = c.object_id
            INNER JOIN
                sys.types AS ty ON c.user_type_id = ty.user_type_id
            ORDER BY
                s.name, t.name, c.column_id;");

            foreach (var group in columns.GroupBy(x => x.TableName))
            {
                var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Edm");
                Directory.CreateDirectory(outputDirectory);

                var propsString = new StringBuilder();
                foreach (var item in group)
                {
                    propsString.AppendLine(
                        Constants.EDM_PROPERTY_TEMPLATE
                            .Replace("$TYPE", new SqlServerSqlTransformer().GetCSharpType(item.DataType, item?.MaxLength ?? "50", item?.IsNullable ?? false))
                            .Replace("$NAME", item?.ColumnName));
                }

                File.WriteAllText(
                    Path.Combine(outputDirectory, $"{group.Key}.cs"), 
                    Constants.EDM_CLASS_TEMPLATE
                        .Replace("$NAMESPACE", configs?.First()?.Databases?.First().Namespace ?? "-")
                        .Replace("$CLASSNAME", group.Key)
                        .Replace("$PROPERTIES", propsString.ToString()));
            }
        }

        "Entity Data Models successfully synchronized.".Success();
        return 0;
    }

    private static bool TryParseConfig(string configFile, out DbBrokerConfig? config)
    {
        config = null;
        try
        {
            var json = File.ReadAllText(configFile);
            config = JsonSerializer.Deserialize<DbBrokerConfig>(json, _jsonSerializerOptions);
            return true;
        }
        catch (Exception ex)
        {
            $"    > Error deserializing '{configFile}': {ex.Message}".Error();
            return false;
        }
    }
}
