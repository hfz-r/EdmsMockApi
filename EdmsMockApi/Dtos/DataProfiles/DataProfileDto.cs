using System.Collections.Generic;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.DataProfiles
{
    [JsonObject(Title = "data_profile")]
    public class DataProfileDto
    {
        private ICollection<DataColumnDto> _dataColumns;

        /// <summary>
        /// EDMS version id
        /// </summary>
        /// <example>1</example>
        [JsonProperty("ver_id")]
        public long VerID { get; set; }

        /// <summary>
        /// Current profile identifier
        /// </summary>
        /// <example>9</example>
        [JsonProperty("profile_id")]
        public long ProfileID { get; set; }

        /// <summary>
        /// Document identifier
        /// </summary>
        /// <example>1</example>
        [JsonProperty("doc_id")]
        public long DocID { get; set; }

        /// <summary>
        /// Profile image name 
        /// </summary>
        /// <example>pp2.jpg</example>
        [JsonProperty("image_name")]
        public string ImageName { get; set; }

        /// <summary>
        /// Total records found
        /// </summary>
        /// <example>0</example>
        [JsonProperty("total_rec_found")]
        public int TotalRecFound { get; set; }

        [JsonProperty("data_columns")]
        public ICollection<DataColumnDto> DataColumns
        {
            get => _dataColumns ?? (_dataColumns = new List<DataColumnDto>());
            set => _dataColumns = value;
        }
    }
}