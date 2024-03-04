using Azure.Storage.Blobs;
namespace RandomDataFetcher.Infrastructure.BlobStorage;

public abstract class BlobStorageSession
{
    protected readonly string ConnectionString;
    protected BlobContainerClient? BlobContainerClient;

    protected BlobServiceClient? BlobServiceClient;

    protected BlobStorageSession(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("BlobStorage connection string cannot be null or empty.", nameof(connectionString));
        }

        ConnectionString = connectionString;
    }

    protected BlobContainerClient GetBlobContainerClient(string containerName)
    {
        BlobServiceClient = new BlobServiceClient(ConnectionString);
        BlobContainerClient = BlobServiceClient.GetBlobContainerClient(containerName);
        BlobContainerClient.CreateIfNotExists();
        return BlobContainerClient;
    }
}
