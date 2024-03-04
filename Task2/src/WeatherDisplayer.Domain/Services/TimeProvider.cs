using WeatherDisplayer.Domain.Interfaces;
namespace WeatherDisplayer.Domain.Services;

public sealed class TimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
