namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface PolicyDescription
    {
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("pattern")]
        string Pattern { get; }
        
        [JsonPropertyName("apply-to")]
        string ApplyTo { get; }
        
        [JsonPropertyName("definition")]
        PolicyDetails Details { get; }
        
        [JsonPropertyName("priority")]
        long Priority { get; }
    }
}