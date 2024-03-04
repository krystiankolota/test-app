using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Application.Common.Settings;
using WeatherDisplayer.Application.Service;
namespace WeatherDisplayer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWeatherService, WeatherService>();

        services.Configure<WeatherApiClientSettings>(configuration.GetSection(WeatherApiClientSettings.SectionName));

        return services;
    }
}
