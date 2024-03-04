namespace WeatherDisplayer.Contracts;

public class WeatherByTemperatureInfo
{
    public double MinTemperature { get; set; }
    public double MaxWindSpeed { get; set; }
    public string Country { get; set; } = null!;
}
