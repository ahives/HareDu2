namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface BindingInfo
    {
        /// <summary>
        /// Name of the source exchange.
        /// </summary>
        [JsonPropertyName("source")]
        string Source { get; }
        
        /// <summary>
        /// Name of the RabbitMQ virtual host object.
        /// </summary>
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        /// <summary>
        /// Name of the destination exchange/queue object.
        /// </summary>
        [JsonPropertyName("destination")]
        string Destination { get; }
        
        /// <summary>
        /// Qualifies the destination object by defining the type of object (e.g., queue, exchange, etc.).
        /// </summary>
        [JsonPropertyName("destination_type")]
        string DestinationType { get; }
        
        [JsonPropertyName("routing_key")]
        string RoutingKey { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonPropertyName("properties_key")]
        string PropertiesKey { get; }
    }
}