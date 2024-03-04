namespace RandomDataFetcher.Application.Common.Interfaces.Infrastructure;

public interface IRandomLogsPayloadStorage
{
    Task<string> GetAsync(string url);
    Task<string> GetAsync(Guid id);
    Task<(Guid Id, string BlobUrl)> StoreAsync(string payloadJson);
}
