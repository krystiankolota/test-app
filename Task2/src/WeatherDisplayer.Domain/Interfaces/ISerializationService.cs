namespace WeatherDisplayer.Domain.Interfaces;

public interface ISerializationService<TSerialized>
{
    T? Deserialize<T>(TSerialized serialized);
    TSerialized Serialize(object data);
}
