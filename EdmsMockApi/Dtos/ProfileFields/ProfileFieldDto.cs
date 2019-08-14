using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.ProfileFields
{
    [JsonObject(Title = "profile_field")]
    public class ProfileFieldDto
    {
        /// <summary>
        /// Column identifier
        /// </summary>
        /// <example>10</example>
        [JsonProperty("col_id")]
        public int ColId { get; set; }

        /// <summary>
        /// Column name
        /// </summary>
        /// <example>Field4</example>
        [JsonProperty("col_name")]
        public string ColName { get; set; }

        /// <summary>
        /// Column description
        /// </summary>
        /// <example>Student ID</example>
        [JsonProperty("col_desc")]
        public string ColDesc { get; set; }

        /// <summary>
        /// Column data-type
        /// </summary>
        /// <example>nvarchar</example>
        [JsonProperty("col_datatype")]
        public string ColDataType { get; set; }
    }
}