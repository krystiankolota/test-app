using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Infrastructure.Database;
using WeatherDisplayer.Infrastructure.Services;
namespace WeatherDisplayer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistance(configuration);
        services.AddScoped<IWeatherApiClient, WeatherApiClient>();
        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredSection("WeatherDatabase:ConnectionString").Value;
        services.AddDbContext<WeatherDataContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IWeatherDataRepository, WeatherDataRepository>();
        return services;
    }
}
