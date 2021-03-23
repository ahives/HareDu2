namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ApplicationImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}