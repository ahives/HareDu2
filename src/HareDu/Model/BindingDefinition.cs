namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BindingDefinition
    {
        public BindingDefinition(string routingKey, IDictionary<string, object> arguments)
        {
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        [JsonPropertyName("routing_key")]
        public string RoutingKey { get; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; }
    }
}