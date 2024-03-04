using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Application.Common.Interfaces;

public interface IWeatherApiClient
{
    Task<(bool IsSuccess, WeatherData? Payload)> GetAsync(string city);
}
