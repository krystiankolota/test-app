using WeatherDisplayer.Application.Common.Interfaces;
namespace WeatherDisplayer.Web.HostedServices;

public sealed class WeatherDataFetchingService : BaseHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public WeatherDataFetchingService(ILogger<WeatherDataFetchingService> logger, IServiceProvider serviceProvider)
        : base(logger)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task Execute(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var weatherDataService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                await weatherDataService.UpdateAsync();
            }

            await Task.Delay(60000, cancellationToken);
        }
    }
}
