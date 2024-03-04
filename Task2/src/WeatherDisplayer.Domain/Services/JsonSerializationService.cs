using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherDisplayer.Domain.Interfaces;
namespace WeatherDisplayer.Domain.Services;

public class JsonSerializationService : ISerializationService<string>
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = {new JsonStringEnumConverter()}
    };

    public T? Deserialize<T>(string serialized)
    {
        return JsonSerializer.Deserialize<T>(serialized, _options);
    }

    string ISerializationService<string>.Serialize(object data)
    {
        return JsonSerializer.Serialize(data, _options);
    }
}
