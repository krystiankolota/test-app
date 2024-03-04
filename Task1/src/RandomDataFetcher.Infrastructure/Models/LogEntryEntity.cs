using Azure;
using Azure.Data.Tables;
using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Infrastructure.Models;

internal class LogEntryEntity : LogEntry, ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
internal static class TableEntityExtensions
{
    public static LogEntry ToDomainModel(this LogEntryEntity entity)
    {
        return new LogEntry
        {
            Id = entity.Id,
            Success = entity.Success,
            Timestamp = entity.Timestamp?.DateTime ?? default,
            PayloadUri = entity.PayloadUri
        };
    }
}
