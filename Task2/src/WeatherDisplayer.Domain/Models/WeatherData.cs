namespace WeatherDisplayer.Domain.Models;

public class WeatherData
{
    public int Id { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public double Temperature { get; set; }
    public int Clouds { get; set; }
    public double WindSpeed { get; set; }
    public DateTime LastUpdate { get; set; }
}
