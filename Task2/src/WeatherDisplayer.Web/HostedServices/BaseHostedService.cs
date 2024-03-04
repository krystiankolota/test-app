namespace WeatherDisplayer.Web.HostedServices;

public abstract class BaseHostedService : BackgroundService
{
    protected ILogger Logger { get; }

    protected BaseHostedService(ILogger logger)
    {
        Logger = logger;
    }
    public int Instance { get; set; }

    protected abstract Task Execute(CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Execute(cancellationToken);
        }
        catch (Exception e)
        {
            await OnError(e);
        }
    }

    protected virtual Task OnError(Exception e)
    {
        Logger.LogError(e, "Hosted Service [{0}]({1}) execution error.", GetType().Name, Instance);
        return Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Hosted Service [{0}]({1}) started.", GetType().Name, Instance);

        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Hosted Service [{0}]({1}) stopped.", GetType().Name, Instance);
        await base.StopAsync(stoppingToken);
    }
}
