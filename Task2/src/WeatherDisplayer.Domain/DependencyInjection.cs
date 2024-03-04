using Microsoft.Extensions.DependencyInjection;
using WeatherDisplayer.Domain.Interfaces;
using WeatherDisplayer.Domain.Services;
namespace WeatherDisplayer.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<ISerializationService<string>, JsonSerializationService>();
        services.AddSingleton<ITimeProvider, TimeProvider>();
        return services;
    }
}
