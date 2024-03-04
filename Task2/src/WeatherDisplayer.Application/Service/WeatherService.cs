using Microsoft.Extensions.Options;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Application.Common.Settings;
using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Interfaces;
using WeatherDisplayer.Domain.Models;
namespace WeatherDisplayer.Application.Service;

public class WeatherService : IWeatherService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly WeatherApiClientSettings _weatherApiClientSettings;
    private readonly IWeatherDataRepository _weatherDataRepository;
    private readonly ITimeProvider _timeProvider;

    public WeatherService(
        IWeatherApiClient weatherApiClient,
        IOptions<WeatherApiClientSettings> weatherApiClientSettings,
        IWeatherDataRepository weatherDataRepository,
        ITimeProvider timeProvider)
    {
        _weatherApiClient = weatherApiClient;
        _weatherDataRepository = weatherDataRepository;
        _timeProvider = timeProvider;
        _weatherApiClientSettings = weatherApiClientSettings.Value;
    }

    public async Task<IList<WeatherData>> GetAsync(string cityName)
    {
        var result = await _weatherDataRepository.GetAsync(
            wd => wd.City == cityName,
            o => o.OrderByDescending(w => w.LastUpdate),
            true);
        return result;
    }

    public async Task UpdateAsync()
    {
        var records = await GetLatestDataAsync();

        foreach (var record in records)
        {
            record.LastUpdate = _timeProvider.UtcNow;
            await _weatherDataRepository.UpdateAsync(record);
        }

        await _weatherDataRepository.SaveAsync();
    }

    public async Task<List<MaxWindSpeedInfo>> GetMaxWindSpeedByCountryAsync(string countryName)
    {
        return await _weatherDataRepository.GetMaxWindSpeedByCountryAsync(countryName);
    }

    public async Task<List<WeatherByTemperatureInfo>> GetWeatherDataByTemperatureAsync(float temperature)
    {
        return await _weatherDataRepository.GetWeatherDataByTemperatureAsync(temperature);
    }

    public async Task<IList<WeatherData>> GetTrends(string city)
    {
        var twoHoursAgo = _timeProvider.UtcNow.AddHours(-2);

        var data = await _weatherDataRepository.GetAsync(
            w => w.City == city && w.LastUpdate >= twoHoursAgo,
            w => w.OrderBy(o => o.LastUpdate),
            true);

        return data;
    }

    public async Task<SummaryResponse> GetSummaryAsync()
    {
        var latestRecords = await _weatherDataRepository.GetLatestRecordsForEachCityAsync();

        var minTemperature = latestRecords
            .GroupBy(w => new {w.Country, w.City})
            .Select(g => new SummaryDto
            {
                Country = g.Key.Country,
                City = g.Key.City,
                Temperature = g.Min(w => w.Temperature),
                LastUpdate = g.Max(w => w.LastUpdate)

            })
            .OrderBy(w => w.Country)
            .ThenBy(w => w.City)
            .ToList();

        var maxWindSpeed = latestRecords
            .GroupBy(w => new {w.Country, w.City})
            .Select(g => new SummaryDto
            {
                Country = g.Key.Country,
                City = g.Key.City,
                WindSpeed = g.Max(w => w.WindSpeed),
                LastUpdate = g.Max(w => w.LastUpdate)
            })
            .OrderBy(w => w.Country)
            .ThenBy(w => w.City)
            .ToList();

        return new SummaryResponse(minTemperature, maxWindSpeed);
    }

    private async Task<IList<WeatherData>> GetLatestDataAsync()
    {
        var weatherApiRequests = _weatherApiClientSettings.SupportedCities
            .Select(city => _weatherApiClient.GetAsync(city))
            .ToList();

        await Task.WhenAll(weatherApiRequests);

        return weatherApiRequests
            .Where(result => result.Result.IsSuccess)
            .Select(result => result.Result.Payload)
            .ToList()!;
    }
}
