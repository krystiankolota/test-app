using System.Text.Json.Serialization;
namespace WeatherDisplayer.Infrastructure.Models;

internal sealed record WeatherApiResponse(
    WeatherApiLocation Location,
    WeatherApiData Current);
internal sealed record WeatherApiLocation(
    string Name,
    string Country);
internal sealed record WeatherApiData
{
    [JsonPropertyName("temp_c")]
    public double Temperature { get; set; }
    [JsonPropertyName("cloud")]
    public int Clouds { get; set; }
    [JsonPropertyName("wind_kph")]
    public double WindSpeed { get; set; }
}
