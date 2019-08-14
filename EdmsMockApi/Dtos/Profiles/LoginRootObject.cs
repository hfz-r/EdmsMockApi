using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    public class LoginRootObject
    {
        /// <summary>
        /// Login success/failed
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}