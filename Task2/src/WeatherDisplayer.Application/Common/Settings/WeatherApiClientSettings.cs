namespace WeatherDisplayer.Application.Common.Settings;

public class WeatherApiClientSettings
{
    public const string SectionName = nameof(WeatherApiClientSettings);

    public string ApiKey { get; set; } = null!;
    public string[] SupportedCities { get; set; } = null!;
}
