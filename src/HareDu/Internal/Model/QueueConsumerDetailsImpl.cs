namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class QueueConsumerDetailsImpl
    {
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}