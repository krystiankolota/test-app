using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Application.Common.Settings;
using WeatherDisplayer.Domain.Exceptions;
using WeatherDisplayer.Domain.Interfaces;
using WeatherDisplayer.Domain.Models;
using WeatherDisplayer.Infrastructure.Models;
namespace WeatherDisplayer.Infrastructure.Services;

public sealed class WeatherApiClient : IWeatherApiClient
{
    private const string ApiHost = "http://api.weatherapi.com/v1/";
    private const string ApiPath = "current.json";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISerializationService<string> _serializationService;
    private readonly WeatherApiClientSettings _weatherApiClientSettings;

    public WeatherApiClient(
        IHttpClientFactory httpClientFactory,
        ISerializationService<string> serializationService,
        IOptions<WeatherApiClientSettings> weatherApiClientSettings)
    {
        _httpClientFactory = httpClientFactory;
        _serializationService = serializationService;
        _weatherApiClientSettings = weatherApiClientSettings.Value;
    }

    public async Task<(bool IsSuccess, WeatherData? Payload)> GetAsync(string city)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(_weatherApiClientSettings.ApiKey);

        var client = Create(ApiHost);
        var response = await client.GetAsync($"{ApiPath}?key={_weatherApiClientSettings.ApiKey}&q={city}");
        WeatherData? weatherData = null;

        if (!response.IsSuccessStatusCode)
        {
            return ValueTuple.Create(response.IsSuccessStatusCode, weatherData);
        }

        var payloadString = await response.Content.ReadAsStringAsync();
        var deserializedPayload = _serializationService.Deserialize<WeatherApiResponse>(payloadString) ?? throw new InvalidApiResponse("Api response is invalid");

        weatherData = new WeatherData
        {
            City = deserializedPayload.Location.Name,
            Country = deserializedPayload.Location.Country,
            Clouds = deserializedPayload.Current.Clouds,
            Temperature = deserializedPayload.Current.Temperature,
            WindSpeed = deserializedPayload.Current.WindSpeed
        };

        return ValueTuple.Create(response.IsSuccessStatusCode, weatherData);
    }

    internal static void ConfigureHttpClient(HttpClient httpClient, string host)
    {
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("Host", new Uri(host).Host);
    }

    internal HttpClient Create(string host)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(WeatherApiClient));
        httpClient.BaseAddress = new Uri(host);
        ConfigureHttpClient(httpClient, host);
        return httpClient;
    }
}
