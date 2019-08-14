using System;

namespace EdmsMockApi.Dtos
{
    public interface ISerializableObject
    {
        string GetPrimaryPropertyName();

        Type GetPrimaryPropertyType();
    }
}