using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Application.Services.RandomLogs.Commands.CreateLogCommand;
using RandomDataFetcher.Domain.Common.Interfaces;
using RandomDataFetcher.FunctionApp.Common.Constants;
namespace RandomDataFetcher.FunctionApp.Functions;

public class RandomDataFetcher
{
    private readonly IRandomApiClient _randomApiClient;
    private readonly ISender _mediator;
    private readonly ITimeProvider _timeProvider;
    private readonly ILogger<RandomDataFetcher> _logger;

    public RandomDataFetcher(
        IRandomApiClient randomApiClient,
        ISender mediator,
        ITimeProvider timeProvider,
        ILogger<RandomDataFetcher> logger)
    {
        _randomApiClient = randomApiClient;
        _mediator = mediator;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    [FunctionName(FunctionNames.RandomDataFetcher)]
    public async Task Run([TimerTrigger(QuartzExpressions.EveryMinute)] TimerInfo myTimer)
    {
        try
        {
            var (isSuccess, payload) = await _randomApiClient.GetDataAsync();
            var createLogCommand = new CreateLogCommand(payload, isSuccess, _timeProvider.UtcNow);
            await _mediator.Send(createLogCommand, CancellationToken.None);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during {FunctionName}", FunctionNames.RandomDataFetcher);
            throw;
        }
    }
}
