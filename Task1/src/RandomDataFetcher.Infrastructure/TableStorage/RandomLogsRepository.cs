using Microsoft.Extensions.Options;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Domain.Common.Interfaces;
using RandomDataFetcher.Domain.Models;
using RandomDataFetcher.Infrastructure.Models;
using RandomDataFetcher.Infrastructure.Settings;
namespace RandomDataFetcher.Infrastructure.TableStorage;

public sealed class RandomLogsRepository : BaseTableStorageService, IRandomLogsRepository
{
    private const string LogPartitionKey = "log";
    private readonly ITimeProvider _timeProvider;

    public RandomLogsRepository(IOptions<LogRepositorySettings> logRepositorySettings, ITimeProvider timeProvider)
        : base(logRepositorySettings.Value.ConnectionString, logRepositorySettings.Value.TableName)
    {
        _timeProvider = timeProvider;
    }

    public async Task<LogEntry> UpsertAsync(
        bool status,
        string payloadUrl,
        Guid? id,
        DateTime? timeStamp)
    {
        id ??= Guid.NewGuid();
        timeStamp ??= _timeProvider.UtcNow;

        var logEntryToBeCreated = new LogEntryEntity
        {
            PartitionKey = LogPartitionKey,
            RowKey = id.ToString()!,
            Id = id.Value,
            PayloadUri = payloadUrl,
            Timestamp = timeStamp,
            Success = status
        };

        await UpsertEntityAsync(logEntryToBeCreated);

        return logEntryToBeCreated.ToDomainModel();
    }

    public async Task<IList<LogEntry>> GetAsync(DateTime from, DateTime to)
    {
        var queryResults = await GetEntitiesAsync<LogEntryEntity>(e =>
            e.PartitionKey == LogPartitionKey &&
            e.Timestamp >= from &&
            e.Timestamp <= to);

        return queryResults
            .Select(entry => entry.ToDomainModel())
            .ToList();
    }
}
