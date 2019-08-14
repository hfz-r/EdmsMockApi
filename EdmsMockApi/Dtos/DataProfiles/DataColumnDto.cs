using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.DataProfiles
{
    [JsonObject(Title = "data_column")]
    public class DataColumnDto
    {
        /// <summary>
        /// Column description
        /// </summary>
        /// <example>Matrix Number</example>
        [JsonProperty("col_desc")]
        public string Col_Desc { get; set; }

        /// <summary>
        /// Current value by this column
        /// </summary>
        /// <example>MSU001</example>
        [JsonProperty("profile_value")]
        public string ProfileValue { get; set; }

        /// <summary>
        /// Column identifier
        /// </summary>
        /// <example>50</example>
        [JsonProperty("col_id")]
        public string Col_ID { get; set; }

        /// <summary>
        /// Column name
        /// </summary>
        /// <example>Field1</example>
        [JsonProperty("col_name")]
        public string Col_Name { get; set; }
    }
}