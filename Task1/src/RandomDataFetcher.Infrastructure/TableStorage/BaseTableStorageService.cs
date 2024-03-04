using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;
namespace RandomDataFetcher.Infrastructure.TableStorage;

public abstract class BaseTableStorageService
{
    private const int MaxNumberOfRetries = 5;
    private readonly TableClient _tableClient;

    protected BaseTableStorageService(string connectionString, string tableName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
        }

        _tableClient = new TableClient(connectionString, tableName);
    }

    public async Task<TEntity> UpsertEntityAsync<TEntity>(TEntity tableEntity)
        where TEntity : ITableEntity
    {
        await RetryAsync(async () => await _tableClient.UpsertEntityAsync(tableEntity));
        return tableEntity;
    }

    public async Task<IList<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class, ITableEntity
    {
        await _tableClient.CreateIfNotExistsAsync();

        var entities = new List<TEntity>();
        await foreach (var entity in _tableClient.QueryAsync(filter))
        {
            entities.Add(entity);
        }

        return entities;
    }

    private async Task RetryAsync(Func<Task> action)
    {
        for (var attempt = 0; attempt <= MaxNumberOfRetries; attempt++)
        {
            try
            {
                await action();
                break;
            }
            catch (RequestFailedException ex) when (attempt < MaxNumberOfRetries && ex.ErrorCode == "TableNotFound")
            {
                await _tableClient.CreateIfNotExistsAsync();
            }
            catch (RequestFailedException) when (attempt == MaxNumberOfRetries)
            {
                throw new Exception($"Operation failed after {MaxNumberOfRetries} retries.");
            }
        }
    }
}
