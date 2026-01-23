namespace Hoteling.Infrastructure.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool UsePostgres { get; set; } = false;
    public string PostgresConnection { get; set; } = string.Empty;
}