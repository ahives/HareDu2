namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class NodeContextImpl
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
        
        [JsonPropertyName("port")]
        public string Port { get; set; }
    }
}