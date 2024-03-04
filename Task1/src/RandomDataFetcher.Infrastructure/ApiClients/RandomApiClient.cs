using System.Net.Http.Headers;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
namespace RandomDataFetcher.Infrastructure.ApiClients;

public sealed class RandomApiClient : IRandomApiClient
{
    private const string ApiHost = "https://api.publicapis.org/";
    private const string ApiPath = "random?auth=null";

    private readonly IHttpClientFactory _httpClientFactory;

    public RandomApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(bool IsSuccess, string? Payload)> GetDataAsync()
    {
        var client = Create(ApiHost);
        var response = await client.GetAsync(ApiPath);
        string? payload = null;

        if (response.IsSuccessStatusCode)
        {
            payload = await response.Content.ReadAsStringAsync();
        }

        return ValueTuple.Create(response.IsSuccessStatusCode, payload);
    }

    internal static void ConfigureHttpClient(HttpClient httpClient, string host)
    {
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("Head", new Uri(host).Host);
    }

    internal HttpClient Create(string host)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(RandomApiClient));
        httpClient.BaseAddress = new Uri(host);
        ConfigureHttpClient(httpClient, host);
        return httpClient;
    }
}
