using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    public class ExportRootObject
    {
        /// <summary>
        /// Export result
        /// </summary>
        [JsonProperty("export_result")]
        public string ExportResult { get; set; }
    }
}