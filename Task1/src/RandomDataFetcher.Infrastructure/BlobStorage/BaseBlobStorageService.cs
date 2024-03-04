using RandomDataFetcher.Domain.Exceptions;
using RandomDataFetcher.Infrastructure.Common.Interfaces;
namespace RandomDataFetcher.Infrastructure.BlobStorage;

public abstract class BaseBlobStorageService : BlobStorageSession, IBaseBlobStorageService
{
    protected BaseBlobStorageService(string connectionString)
        : base(connectionString)
    {
    }

    public async Task<BinaryData> GetAsync(string container, string filePath)
    {
        var containerClient = GetBlobContainerClient(container);
        var blobClient = containerClient.GetBlobClient(filePath);
        var content = await blobClient.DownloadContentAsync();

        if (!content.HasValue)
        {
            throw new BlobOrBlobContainerNotFound($"{container} or {filePath} was not found in blob storage");
        }

        return content.Value.Content;
    }

    public async Task<BinaryData> GetAsync(Uri url)
    {
        var (containerName, filePath) = ParseBlobUri(url);
        var blobClient = GetBlobContainerClient(containerName).GetBlobClient(filePath);

        if (!await blobClient.ExistsAsync())
        {
            throw new BlobOrBlobContainerNotFound($"{blobClient.Uri} does not exist");
        }

        var content = await blobClient.DownloadContentAsync();
        return content.Value.Content;
    }

    public async Task<string> UpsertAsync(BinaryData content, string fileName, string container)
    {
        var blobContainerClient = GetBlobContainerClient(container);
        await blobContainerClient.DeleteBlobIfExistsAsync(fileName);
        _ = await blobContainerClient.UploadBlobAsync(fileName, content);
        return $"{blobContainerClient.Uri}/{fileName}";
    }

    private static (string ContainerName, string FilePath) ParseBlobUri(Uri uri)
    {
        var segments = uri.AbsolutePath.TrimStart('/').Split('/');

        if (segments.Length < 2)
        {
            throw new ArgumentException("The provided Blob URL does not contain enough segments to determine the container name and file path.", nameof(uri));
        }

        var skip = segments[0] == "devstoreaccount1" ? 2 : 1;
        var containerName = segments[skip - 1];
        var filePath = string.Join("/", segments.Skip(skip));

        return (containerName, filePath);
    }
}
