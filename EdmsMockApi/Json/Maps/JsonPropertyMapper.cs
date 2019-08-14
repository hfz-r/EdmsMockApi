using System;
using System.Collections.Generic;
using System.Reflection;
using EdmsMockApi.Caching;
using EdmsMockApi.Infrastructure.Attributes;
using Newtonsoft.Json;

namespace EdmsMockApi.Json.Maps
{
    public class JsonPropertyMapper : IJsonPropertyMapper
    {
        private readonly ICacheManager _cacheManager;

        public JsonPropertyMapper(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public Dictionary<string, Tuple<string, Type>> GetMap(Type type)
        {
            if (!_cacheManager.IsSet(Configurations.JsonTypeMapsPattern))
                _cacheManager.Set(Configurations.JsonTypeMapsPattern,
                    new Dictionary<string, Dictionary<string, Tuple<string, Type>>>(), int.MaxValue);

            var typeMaps =
                _cacheManager.Get<Dictionary<string, Dictionary<string, Tuple<string, Type>>>>(
                    Configurations.JsonTypeMapsPattern, () => null, 0);

            if (!typeMaps.ContainsKey(type.Name))
                Build(type);

            return typeMaps[type.Name];
        }

        private void Build(Type type)
        {
            var typeMaps =
                _cacheManager.Get<Dictionary<string, Dictionary<string, Tuple<string, Type>>>>(
                    Configurations.JsonTypeMapsPattern, () => null, 0);

            var mapForCurrentType = new Dictionary<string, Tuple<string, Type>>();

            foreach (var property in type.GetProperties())
            {
                var jsonAttribute = property.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
                var doNotMapAttribute = property.GetCustomAttribute(typeof(DoNotMapAttribute)) as DoNotMapAttribute;

                if (jsonAttribute != null && doNotMapAttribute == null)
                {
                    if (!mapForCurrentType.ContainsKey(jsonAttribute.PropertyName))
                    {
                        var value = new Tuple<string, Type>(property.Name, property.PropertyType);
                        mapForCurrentType.Add(jsonAttribute.PropertyName, value);
                    }
                }
            }

            if (!typeMaps.ContainsKey(type.Name))
                typeMaps.Add(type.Name, mapForCurrentType);
        }
    }
}