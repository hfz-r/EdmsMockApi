using System.Collections.Generic;
using EdmsMockApi.Dtos.ProfileFields;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    [JsonObject(Title = "profile")]
    public class ProfileDto
    {
        private ICollection<ProfileFieldDto> _profileFields;

        /// <summary>
        /// Profile identifier
        /// </summary>
        /// <example>9</example>
        [JsonProperty("profile_id")]
        public int? ProfileId { get; set; }

        /// <summary>
        /// Profile name
        /// </summary>
        /// <example>MSU Student Records</example>
        [JsonProperty("profile_name")]
        public string ProfileName { get; set; }

        [JsonProperty("profile_fields")]
        public ICollection<ProfileFieldDto> ProfileFields
        {
            get => _profileFields ?? (_profileFields = new List<ProfileFieldDto>());
            set => _profileFields = value;
        }
    }
}