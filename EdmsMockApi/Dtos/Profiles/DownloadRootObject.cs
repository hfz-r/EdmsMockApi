using System;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    public class DownloadRootObject : ISerializableObject
    {
        public DownloadRootObject()
        {
            Download = new DownloadDto();
        }

        /// <summary>
        /// Download object
        /// </summary>
        [JsonProperty("download")]
        public DownloadDto Download { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "download";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(DownloadDto);
        }
    }
}