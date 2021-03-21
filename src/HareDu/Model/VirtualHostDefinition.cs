namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface VirtualHostDefinition
    {
        [JsonPropertyName("tracing")]
        bool Tracing { get; }
        
        [JsonPropertyName("description")]
        string Description { get; }
        
        [JsonPropertyName("tags")]
        string Tags { get; }
    }
}