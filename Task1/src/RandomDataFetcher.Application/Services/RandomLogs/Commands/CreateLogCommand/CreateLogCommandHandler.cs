using MediatR;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Services.RandomLogs.Commands.CreateLogCommand;

public sealed class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, LogEntry>
{
    private readonly IRandomLogsRepository _randomLogsRepository;
    private readonly IRandomLogsPayloadStorage _randomLogsPayloadStorage;

    public CreateLogCommandHandler(IRandomLogsRepository randomLogsRepository, IRandomLogsPayloadStorage randomLogsPayloadStorage)
    {
        _randomLogsRepository = randomLogsRepository;
        _randomLogsPayloadStorage = randomLogsPayloadStorage;
    }

    public async Task<LogEntry> Handle(CreateLogCommand request, CancellationToken cancellationToken)
    {
        var (id, blobUrl) = await _randomLogsPayloadStorage.StoreAsync(request.Payload);
        var createdLogEntry = await _randomLogsRepository.UpsertAsync(
            request.IsSuccess,
            blobUrl,
            id,
            request.TimeStamp);
        return createdLogEntry;
    }
}
