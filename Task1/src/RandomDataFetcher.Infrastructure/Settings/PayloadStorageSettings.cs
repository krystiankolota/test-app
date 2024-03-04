namespace RandomDataFetcher.Infrastructure.Settings;

public class PayloadStorageSettings
{
    public const string SectionName = nameof(PayloadStorageSettings);

    public string ConnectionString { get; set; } = null!;
    public string ContainerName { get; set; } = null!;
}
