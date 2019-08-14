using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.DataProfiles
{
    public class DataProfilesRootObject : ISerializableObject
    {
        public DataProfilesRootObject()
        {
            DataProfiles = new List<DataProfileDto>();
        }

        /// <summary>
        /// Collection of data profiles object
        /// </summary>
        [JsonProperty("data_profiles")]
        public IList<DataProfileDto> DataProfiles { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "data_profiles";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(DataProfileDto);
        }
    }
}