using EdmsMockApi.Dtos;

namespace EdmsMockApi.Json.Serializer
{
    public interface IJsonFieldsSerializer
    {
        string Serialize(ISerializableObject objectToSerialize, string jsonFields);
    }
}