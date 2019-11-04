using Newtonsoft.Json;

namespace EdmsMockApi.Dtos.Profiles
{
    [JsonObject(Title = "download")]
    public class DownloadDto
    {
        [JsonProperty("download_result")]
        public string Result { get; set; }

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("file_content")]
        public byte[] Content { get; set; }
    }
}