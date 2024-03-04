using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Infrastructure.ApiClients;
using RandomDataFetcher.Infrastructure.BlobStorage;
using RandomDataFetcher.Infrastructure.Settings;
using RandomDataFetcher.Infrastructure.TableStorage;
namespace RandomDataFetcher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExternalApi();
        services.AddPersistence();

        services.Configure<PayloadStorageSettings>(configuration.GetSection(PayloadStorageSettings.SectionName));
        services.Configure<LogRepositorySettings>(configuration.GetSection(LogRepositorySettings.SectionName));
        return services;
    }

    private static void AddExternalApi(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IRandomApiClient, RandomApiClient>();
    }

    private static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IRandomLogsPayloadStorage, RandomLogsPayloadPayloadStorage>();
        services.AddScoped<IRandomLogsRepository, RandomLogsRepository>();
    }
}
