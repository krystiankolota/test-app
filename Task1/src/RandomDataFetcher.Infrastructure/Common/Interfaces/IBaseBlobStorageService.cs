namespace RandomDataFetcher.Infrastructure.Common.Interfaces;

public interface IBaseBlobStorageService
{
    Task<BinaryData> GetAsync(Uri url);
    Task<BinaryData> GetAsync(string container, string filePath);
    Task<string> UpsertAsync(BinaryData content, string fileName, string container);
}
