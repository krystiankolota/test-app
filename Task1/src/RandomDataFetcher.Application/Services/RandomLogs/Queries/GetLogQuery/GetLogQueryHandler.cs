using MediatR;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
namespace RandomDataFetcher.Application.Services.RandomLogs.Queries.GetLogQuery;

public sealed class GetLogQueryHandler : IRequestHandler<GetLogQuery, string>
{
    private readonly IRandomLogsPayloadStorage _randomLogsPayloadStorage;
    public GetLogQueryHandler(IRandomLogsPayloadStorage randomLogsPayloadStorage)
    {
        _randomLogsPayloadStorage = randomLogsPayloadStorage;
    }

    public async Task<string> Handle(GetLogQuery request, CancellationToken cancellationToken)
    {
        return await _randomLogsPayloadStorage.GetAsync(request.Id);
    }
}
