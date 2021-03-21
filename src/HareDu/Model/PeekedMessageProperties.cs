namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface PeekedMessageProperties
    {
        [JsonPropertyName("message_id")]
        string MessageId { get; }
        
        [JsonPropertyName("correlation_id")]
        string CorrelationId { get; }
        
        [JsonPropertyName("delivery_mode")]
        uint DeliveryMode { get; }
        
        [JsonPropertyName("headers")]
        IDictionary<string, object> Headers { get; }
        
        [JsonPropertyName("content_type")]
        string ContentType { get; }
    }
}