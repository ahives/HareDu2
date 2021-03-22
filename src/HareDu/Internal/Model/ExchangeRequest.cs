namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ExchangeRequest
    {
        public ExchangeRequest(ExchangeRoutingType routingType, bool durable, bool autoDelete, bool @internal, IDictionary<string, object> arguments)
        {
            RoutingType = routingType;
            Durable = durable;
            AutoDelete = autoDelete;
            Internal = @internal;
            Arguments = arguments;
        }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ExchangeRoutingType RoutingType { get; }
        
        [JsonPropertyName("durable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Durable { get; }
        
        [JsonPropertyName("auto_delete")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AutoDelete { get; }
        
        [JsonPropertyName("internal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Internal { get; }
        
        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IDictionary<string, object> Arguments { get; }
    }
}