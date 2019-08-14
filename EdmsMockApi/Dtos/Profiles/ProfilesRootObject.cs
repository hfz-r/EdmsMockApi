using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    public class ProfilesRootObject : ISerializableObject
    {
        public ProfilesRootObject()
        {
            Profiles = new List<ProfileDto>();
        }

        /// <summary>
        /// Collection of profiles object 
        /// </summary>
        [JsonProperty("profiles")]
        public IList<ProfileDto> Profiles { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "profiles";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(ProfileDto);
        }
    }
}