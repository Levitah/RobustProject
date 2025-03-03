namespace RobustProject.Services.Options;

public class SqlServerOptions
{
    public const string Section = "SqlServer";

    public string? ConnectionString { get; set; }
    public int ClientRetries { get; set; }
    public int ClientRetryDelayInMilliseconds { get; set; }
}