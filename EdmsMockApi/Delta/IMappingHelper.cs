using System;
using System.Collections.Generic;

namespace EdmsMockApi.Delta
{
    public interface IMappingHelper
    {
        void Merge(object source, object destination);
        void SetValues(Dictionary<string, object> propertyNameValuePairs, object objectToBeUpdated, Type propertyType, Dictionary<object, object> objectPropertyNameValuePairs, bool handleComplexTypeCollections = false);
    }
}