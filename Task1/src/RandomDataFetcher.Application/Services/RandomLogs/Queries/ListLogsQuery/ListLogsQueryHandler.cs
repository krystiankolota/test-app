using MediatR;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Services.RandomLogs.Queries.ListLogsQuery;

public sealed class ListLogsQueryHandler : IRequestHandler<ListLogsQuery, IList<LogEntry>>
{
    private readonly IRandomLogsRepository _randomLogsRepository;

    public ListLogsQueryHandler(IRandomLogsRepository randomLogsRepository)
    {
        _randomLogsRepository = randomLogsRepository;
    }

    public async Task<IList<LogEntry>> Handle(ListLogsQuery request, CancellationToken cancellationToken)
    {
        var queryResult = await _randomLogsRepository.GetAsync(request.From, request.To);
        return queryResult;
    }
}
