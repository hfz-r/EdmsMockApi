using System;
using System.Collections.Generic;

namespace EdmsMockApi.Json.Maps
{
    public interface IJsonPropertyMapper
    {
        Dictionary<string, Tuple<string, Type>> GetMap(Type type);
    }
}