namespace WeatherDisplayer.Domain.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
