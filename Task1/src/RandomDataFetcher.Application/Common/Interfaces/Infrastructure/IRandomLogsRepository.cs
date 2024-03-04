using RandomDataFetcher.Domain.Models;
namespace RandomDataFetcher.Application.Common.Interfaces.Infrastructure;

public interface IRandomLogsRepository
{
    Task<LogEntry> UpsertAsync(bool status, string payloadUrl, Guid? id, DateTime? timeStamp);
    Task<IList<LogEntry>> GetAsync(DateTime from, DateTime to);
}
