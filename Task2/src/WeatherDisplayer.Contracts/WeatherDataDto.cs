namespace WeatherDisplayer.Contracts;

public record WeatherDataDto(
    string Country,
    string City,
    int Clouds,
    double Temperature,
    double WindSpeed,
    DateTime LastUpdate);
