namespace RandomDataFetcher.Infrastructure.Settings;

public class LogRepositorySettings
{
    public const string SectionName = nameof(LogRepositorySettings);

    public string ConnectionString { get; set; } = null!;
    public string TableName { get; set; } = null!;
}
