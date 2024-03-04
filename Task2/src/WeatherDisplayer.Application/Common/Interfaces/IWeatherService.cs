using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Application.Common.Interfaces;

public interface IWeatherService
{
    Task<IList<WeatherData>> GetAsync(string cityName);
    Task UpdateAsync();
    Task<List<MaxWindSpeedInfo>> GetMaxWindSpeedByCountryAsync(string countryName);
    Task<List<WeatherByTemperatureInfo>> GetWeatherDataByTemperatureAsync(float temperature);
    Task<IList<WeatherData>> GetTrends(string city);
    Task<SummaryResponse> GetSummaryAsync();
}
