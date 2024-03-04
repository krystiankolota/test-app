namespace RandomDataFetcher.Application.Common.Interfaces.Infrastructure;

public interface IRandomApiClient
{
    Task<(bool IsSuccess, string? Payload)> GetDataAsync();
}
