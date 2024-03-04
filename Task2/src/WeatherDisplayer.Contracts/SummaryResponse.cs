namespace WeatherDisplayer.Contracts;

public record SummaryResponse(List<SummaryDto> MinTemperature, List<SummaryDto> MaxWindSpeed);
public class SummaryDto
{
    public string Country { get; init; }
    public string City { get; init; }
    public double? Temperature { get; init; }
    public DateTime LastUpdate { get; init; }
    public double? WindSpeed { get; init; }
}
