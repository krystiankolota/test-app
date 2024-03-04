using System.Text.Json;
using System.Text.Json.Serialization;
using RandomDataFetcher.Application.Common.Interfaces.JsonSerialization;
namespace RandomDataFetcher.Application.Services.SerializationService;

public class JsonSerializationService : ISerializationService<string>, ISerializationService<BinaryData>
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = {new JsonStringEnumConverter()}
    };

    public T? Deserialize<T>(BinaryData serialized)
    {
        return JsonSerializer.Deserialize<T>(serialized, _options);
    }

    BinaryData ISerializationService<BinaryData>.Serialize(object data)
    {
        return new BinaryData(data, _options);
    }

    public T? Deserialize<T>(string serialized)
    {
        return JsonSerializer.Deserialize<T>(serialized, _options);
    }

    string ISerializationService<string>.Serialize(object data)
    {
        return JsonSerializer.Serialize(data, _options);
    }
}
