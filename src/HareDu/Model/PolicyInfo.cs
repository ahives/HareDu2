namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface PolicyInfo
    {
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("pattern")]
        string Pattern { get; }
        
        [JsonPropertyName("apply-to")]
        string AppliedTo { get; }

        [JsonPropertyName("definition")]
        IDictionary<string, string> Definition { get; }

        [JsonPropertyName("priority")]
        int Priority { get; }
    }
}