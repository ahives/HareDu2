namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface QueueConsumerDetails
    {
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("name")]
        string Name { get; }
    }
}