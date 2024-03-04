using Microsoft.Extensions.DependencyInjection;
using RandomDataFetcher.Application.Common.Interfaces.JsonSerialization;
using RandomDataFetcher.Application.Services.SerializationService;
namespace RandomDataFetcher.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ISerializationService<string>, JsonSerializationService>();
        services.AddSingleton<ISerializationService<BinaryData>, JsonSerializationService>();
        return services;
    }
}
