using System;
using System.Collections.Generic;
using System.Linq;
using EdmsMockApi.Dtos;
using EdmsMockApi.Helpers;
using Newtonsoft.Json.Linq;

namespace EdmsMockApi.Json.Serializer
{
    public class JsonFieldsSerializer : IJsonFieldsSerializer
    {
        #region Private methods

        private static IList<string> GetPropertiesIntoList(string fields)
        {
            IList<string> properties = fields.ToLowerInvariant()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return properties;
        }

        private static string Serialize(object objectToSerialize, IList<string> jsonFields = null)
        {
            var jToken = JToken.FromObject(objectToSerialize);

            if (jsonFields != null)
                jToken = jToken.RemoveEmptyChildrenAndFilterByFields(jsonFields);

            var jTokenResult = jToken.ToString();

            return jTokenResult;
        }

        #endregion

        public string Serialize(ISerializableObject objectToSerialize, string jsonFields)
        {
            if (objectToSerialize == null)
                throw new ArgumentNullException(nameof(objectToSerialize));

            IList<string> fieldsList = null;

            if (!string.IsNullOrEmpty(jsonFields))
            {
                var primaryPropertyName = objectToSerialize.GetPrimaryPropertyName();

                fieldsList = GetPropertiesIntoList(jsonFields);
                fieldsList.Add(primaryPropertyName);
            }

            var json = Serialize(objectToSerialize, fieldsList);

            return json;
        }
    }
}