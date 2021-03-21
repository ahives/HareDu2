namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface BindingDefinition
    {
        [JsonPropertyName("routing_key")]
        string RoutingKey { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
    }
}