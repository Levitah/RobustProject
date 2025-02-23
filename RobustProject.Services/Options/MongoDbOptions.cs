namespace RobustProject.Services.Options;

public class MongoDbOptions
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? RosebudContainerName { get; set; }
    public int ClientRetries { get; set; }
    public int ClientRetryDelayInMilliseconds { get; set; }
}