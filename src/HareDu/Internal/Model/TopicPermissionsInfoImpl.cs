namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class TopicPermissionsInfoImpl
    {
        [JsonPropertyName("user")]
        public string User { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }
        
        [JsonPropertyName("write")]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        public string Read { get; set; }
    }
}