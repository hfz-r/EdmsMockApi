using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Errors
{
    public class ErrorsRootObject : ISerializableObject
    {
        /// <summary>
        /// Collection of errors description
        /// </summary>
        /// <example>UnsupportedApiVersion</example>
        [JsonProperty("errors")]
        public Dictionary<string, List<string>> Errors { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "errors";
        }

        public Type GetPrimaryPropertyType()
        {
            return Errors.GetType();
        }
    }
}