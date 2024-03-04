using Microsoft.Extensions.DependencyInjection;
using RandomDataFetcher.Domain.Common.Interfaces;
using RandomDataFetcher.Domain.Services;
namespace RandomDataFetcher.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<ITimeProvider, TimeProvider>();
        return services;
    }
}
