namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface TopicPermissionsInfo
    {
        [JsonPropertyName("user")]
        string User { get; }
        
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("exchange")]
        string Exchange { get; }
        
        [JsonPropertyName("write")]
        string Write { get; }
        
        [JsonPropertyName("read")]
        string Read { get; }
    }
}