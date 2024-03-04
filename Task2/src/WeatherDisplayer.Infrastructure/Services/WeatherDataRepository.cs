using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Contracts;
using WeatherDisplayer.Domain.Models;
using WeatherDisplayer.Infrastructure.Database;
namespace WeatherDisplayer.Infrastructure.Services;

public class WeatherDataRepository : BaseRepository<WeatherData>, IWeatherDataRepository
{
    public WeatherDataRepository(WeatherDataContext context)
        : base(context)
    {
    }

    public async Task<List<MaxWindSpeedInfo>> GetMaxWindSpeedByCountryAsync(string countryName)
    {
        return await ExecuteStoredProcedureAsync<MaxWindSpeedInfo>("GetMaxWindSpeedByCountry", "CountryName", countryName);
    }

    public async Task<List<WeatherByTemperatureInfo>> GetWeatherDataByTemperatureAsync(float temperature)
    {
        return await ExecuteStoredProcedureAsync<WeatherByTemperatureInfo>("GetWeatherDataByTemperature", "Temperature", temperature.ToString(CultureInfo.InvariantCulture));
    }

    public async Task<List<WeatherData>> GetLatestRecordsForEachCityAsync()
    {
        var latestEntriesPerCity = await Context.WeatherData
            .GroupBy(w => w.City)
            .Select(g => g.OrderByDescending(w => w.LastUpdate).FirstOrDefault())
            .ToListAsync();
        return latestEntriesPerCity!;
    }
}
