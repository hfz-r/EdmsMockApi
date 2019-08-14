using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.DataProfiles
{
    public class BaseSearchDto
    {
        public BaseSearchDto()
        {
            Order = Configurations.DefaultOrder;
            Query = string.Empty;
            Page = Configurations.DefaultPageValue;
            Limit = Configurations.DefaultLimit;
            Fields = string.Empty;
        }

        /// <summary>
        /// Field and direction to order results by (default: id DESC)
        /// </summary>
        /// <example>matric_no</example>
        [JsonProperty("order")]
        public string Order { get; set; }

        /// <summary>
        /// Text to search profiles
        /// </summary>
        /// <example>image_name:myimage</example>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Page to show (default: 1)
        /// </summary>
        /// <example>5</example>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Amount of results (default: 50) (maximum: 250)
        /// </summary>
        /// <example>10</example>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Comma-separated list of fields to include in the response
        /// </summary>
        /// <example>date_of_birth</example>
        [JsonProperty("fields")]
        public string Fields { get; set; }
    }
}