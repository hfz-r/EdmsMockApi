using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EdmsMockApi.Delta
{
    public class MappingHelper : IMappingHelper
    {
        #region Private methods

        private void SetValue(object objectToBeUpdated, KeyValuePair<string, object> propertyNameValuePair, Dictionary<object, object> objectPropertyNameValuePairs, bool handleComplexTypeCollections)
        {
            var propertyName = propertyNameValuePair.Key;
            var propertyValue = propertyNameValuePair.Value;

            var propertyToUpdate = objectToBeUpdated.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyToUpdate != null)
            {
                //handles nested properties
                if(propertyValue is Dictionary<string, object> objects)
                {
                    var valueToUpdate = propertyToUpdate.GetValue(objectToBeUpdated);

                    SetValues(objects, valueToUpdate, propertyToUpdate.PropertyType, objectPropertyNameValuePairs);
                }
                //handles collections
                else if (propertyValue is ICollection<object>)
                {
                    var propertyValueAsCollection = propertyValue as ICollection<object>;

                    var collectionElementsType = propertyToUpdate.PropertyType.GetGenericArguments()[0];
                    var collection = propertyToUpdate.GetValue(objectToBeUpdated);

                    if (collection == null)
                    {
                        collection = CreateEmptyList(collectionElementsType);
                        propertyToUpdate.SetValue(objectToBeUpdated, collection);
                    }

                    var collectionAsList = (IList) collection;
                    if (collectionAsList == null)
                    {
                        collectionAsList = CreateEmptyList(collectionElementsType);

                        var collectionAsEnumerable = (IEnumerable) collection;
                        foreach (var collectionItem in collectionAsEnumerable)
                            collectionAsList.Add(collectionItem);

                        collection = collectionAsList;
                        propertyToUpdate.SetValue(objectToBeUpdated, collection);
                    }

                    foreach (var item in propertyValueAsCollection)
                    {
                        if (collectionElementsType.Namespace != "System")
                        {
                            if (handleComplexTypeCollections)
                                AddOrUpdateComplexItemInCollection(item as Dictionary<string, object>, collection as IList, collectionElementsType, objectPropertyNameValuePairs, true);
                        }
                        else AddBaseItemInCollection(item, collection as IList, collectionElementsType);
                    }

                    return;
                }

                if (propertyValue == null)
                    propertyToUpdate.SetValue(objectToBeUpdated, null);
                else if (propertyValue is IConvertible)
                    ConvertAndSetValueIfValid(objectToBeUpdated, propertyToUpdate, propertyValue);
                else
                    propertyToUpdate.SetValue(objectToBeUpdated, propertyValue);
            }
        }

        private static IList CreateEmptyList(Type listItemType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(listItemType);
            var list = Activator.CreateInstance(constructedListType);

            return list as IList;
        }

        private static void AddBaseItemInCollection(object newItem, IList collection, Type collectionElementsType)
        {
            var converter = TypeDescriptor.GetConverter(collectionElementsType);

            var newItemValueToString = newItem.ToString();

            if (converter.IsValid(newItemValueToString))
                collection.Add(converter.ConvertFrom(newItemValueToString));
        }

        private void AddOrUpdateComplexItemInCollection(Dictionary<string, object> newProperties, IList collection, Type collectionElementsType, Dictionary<object, object> objectPropertyNameValuePairs, bool handleComplexTypeCollections)
        {
            if (newProperties.ContainsKey("Id"))
            {
                var id = int.Parse(newProperties["Id"].ToString());

                object itemToBeUpdated = null;

                foreach (var item in collection)
                {
                    if (int.Parse(item.GetType()
                            .GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                            .GetValue(item)
                            .ToString()) == id)
                    {
                        itemToBeUpdated = item;
                        break;
                    }
                }

                if (itemToBeUpdated == null)
                    AddNewItemInCollection(newProperties, collection, collectionElementsType, objectPropertyNameValuePairs, handleComplexTypeCollections);
                else
                    SetValues(newProperties, itemToBeUpdated, collectionElementsType, objectPropertyNameValuePairs, handleComplexTypeCollections);
            }
            else AddNewItemInCollection(newProperties, collection, collectionElementsType, objectPropertyNameValuePairs, handleComplexTypeCollections);
        }

        private void AddNewItemInCollection(Dictionary<string, object> newProperties, IList collection, Type collectionElementsType, Dictionary<object, object> objectPropertyNameValuePairs, bool handleComplexTypeCollections)
        {
            var newInstance = Activator.CreateInstance(collectionElementsType);

            SetValues(newProperties, newInstance, collectionElementsType, objectPropertyNameValuePairs, handleComplexTypeCollections);

            collection.Add(newInstance);
        }

        private static void ConvertAndSetValueIfValid(object objectToBeUpdated, PropertyInfo objectProperty, object propertyValue)
        {
            var converter = TypeDescriptor.GetConverter(objectProperty.PropertyType);

            var propertyValueAsString = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", propertyValue);

            if (converter.IsValid(propertyValueAsString))
            {
                var convertedValue = converter.ConvertFromInvariantString(propertyValueAsString);

                objectProperty.SetValue(objectToBeUpdated, convertedValue);
            }
        }

        #endregion

        public void Merge(object source, object destination)
        {
            var sourcePropertyValuePairs = source.GetType()
                .GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(source));

            SetValues(sourcePropertyValuePairs, destination, destination.GetType(), null);
        }

        public void SetValues(Dictionary<string, object> propertyNameValuePairs, object objectToBeUpdated, Type propertyType, Dictionary<object, object> objectPropertyNameValuePairs, bool handleComplexTypeCollections = false)
        {
            objectPropertyNameValuePairs?.Add(objectToBeUpdated, propertyNameValuePairs);

            foreach (var propertyNameValuePair in propertyNameValuePairs)
            {
                SetValue(objectToBeUpdated, propertyNameValuePair, objectPropertyNameValuePairs, handleComplexTypeCollections);
            }
        }

      
    }
}