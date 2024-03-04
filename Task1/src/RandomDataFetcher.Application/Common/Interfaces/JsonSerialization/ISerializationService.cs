namespace RandomDataFetcher.Application.Common.Interfaces.JsonSerialization;

public interface ISerializationService<TSerialized>
{
    T? Deserialize<T>(TSerialized serialized);
    TSerialized Serialize(object data);
}
