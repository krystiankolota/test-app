using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Application.Common.Interfaces;

public interface IWeatherDataRepository : IBaseRepository<WeatherData>
{
    Task<List<MaxWindSpeedInfo>> GetMaxWindSpeedByCountryAsync(string countryName);
    Task<List<WeatherByTemperatureInfo>> GetWeatherDataByTemperatureAsync(float temperature);
    Task<List<WeatherData>> GetLatestRecordsForEachCityAsync();
}
