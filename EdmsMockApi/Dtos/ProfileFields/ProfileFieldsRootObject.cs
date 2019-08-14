using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.ProfileFields
{
    public class ProfileFieldsRootObject : ISerializableObject
    {
        public ProfileFieldsRootObject()
        {
            ProfileFields = new List<ProfileFieldDto>();
        }

        /// <summary>
        /// Collection of profile fields object
        /// </summary>
        [JsonProperty("profile_fields")]
        public IList<ProfileFieldDto> ProfileFields { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "profile_fields";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(ProfileFieldDto);
        }
    }
}