using System.Collections.Generic;

namespace EdmsMockApi.Converters
{
    public interface IObjectConverter
    {
        T ToObject<T>(ICollection<KeyValuePair<string, string>> source) where T : class, new();
    }
}