using System;
using System.Collections;
using System.Collections.Generic;
using EdmsMockApi.Json.Maps;

namespace EdmsMockApi.Delta
{
    public class Delta<TDto> where TDto : class, new()
    {
        private readonly IJsonPropertyMapper _jsonPropertyMapper;
        private readonly Dictionary<string, object> _changedJsonPropertyNames;

        private Dictionary<string, object> _propertyValuePairs;

        private Dictionary<string, object> PropertyValuePairs =>
            _propertyValuePairs ??
            (_propertyValuePairs = GetPropertyValuePairs(typeof(TDto), _changedJsonPropertyNames));

        private IMappingHelper _mappingHelper = new MappingHelper();
        private TDto _dto;

        public Delta(IJsonPropertyMapper jsonPropertyMapper, Dictionary<string, object> changedJsonPropertyNames)
        {
            _jsonPropertyMapper = jsonPropertyMapper;
            _changedJsonPropertyNames = changedJsonPropertyNames;

            _mappingHelper.SetValues(PropertyValuePairs, Dto, typeof(TDto), ObjectPropertyNameValuePairs, true);
        }

        public Dictionary<object, object> ObjectPropertyNameValuePairs = new Dictionary<object, object>();
        public TDto Dto => _dto ?? (_dto = new TDto());

        public void Merge<TEntity>(TEntity entity, bool mergeComplexTypeCollections = true)
        {
            _mappingHelper.SetValues(PropertyValuePairs, entity, entity.GetType(), null, mergeComplexTypeCollections);
        }

        public void Merge<TEntity>(object dto, TEntity entity, bool mergeComplexTypeCollections = true)
        {
            if (dto != null && ObjectPropertyNameValuePairs.ContainsKey(dto))
            {
                var propertyValuePairs = ObjectPropertyNameValuePairs[dto] as Dictionary<string, object>;
                _mappingHelper.SetValues(propertyValuePairs, entity, entity.GetType(), null, mergeComplexTypeCollections);
            }
        }

        private Dictionary<string, object> GetPropertyValuePairs(Type type, Dictionary<string, object> changedJsonPropertyNames)
        {
            var propertyValuePairs = new Dictionary<string, object>();

            if (changedJsonPropertyNames == null)
                return propertyValuePairs;

            var typeMap = _jsonPropertyMapper.GetMap(type);

            foreach (var changedProperty in changedJsonPropertyNames)
            {
                var jsonName = changedProperty.Key;

                if (typeMap.ContainsKey(jsonName))
                {
                    var propertyNameAndType = typeMap[jsonName];

                    var propertyName = propertyNameAndType.Item1;
                    var propertyType = propertyNameAndType.Item2;

                    // Handle system types
                    // This is also the recursion base
                    if (propertyType.Namespace == "System")
                    {
                        propertyValuePairs.Add(propertyName, changedProperty.Value);
                    }
                    // Handle collections
                    else if (propertyType.GetInterface(typeof(IEnumerable).FullName) != null)
                    {
                        if (changedProperty.Value == null)
                            continue;

                        var collection = changedProperty.Value as IEnumerable<object>;
                        var collectionElementsType = propertyType.GetGenericArguments()[0];
                        var resultCollection = new List<object>();

                        foreach (var item in collection)
                        {
                            // Simple types in collection
                            if (collectionElementsType.Namespace == "System")
                            {
                                resultCollection.Add(item);
                            }
                            // Complex types in collection
                            else
                            {
                                var itemDictionary = item as Dictionary<string, object>;

                                resultCollection.Add(GetPropertyValuePairs(collectionElementsType, itemDictionary));
                            }
                        }

                        propertyValuePairs.Add(propertyName, resultCollection);
                    }
                    // Handle nested properties
                    else
                    {
                        var changedPropertyValueDictionary = changedProperty.Value as Dictionary<string, object>;

                        var resultedNestedObject = GetPropertyValuePairs(propertyType, changedPropertyValueDictionary);

                        propertyValuePairs.Add(propertyName, resultedNestedObject);
                    }
                }
            }

            return propertyValuePairs;
        }
    }
}