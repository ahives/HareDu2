namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface ConsumerInfo
    {
        [JsonPropertyName("prefetch_count")]
        ulong PreFetchCount { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonPropertyName("ack_required")]
        bool AcknowledgementRequired { get; }
        
        [JsonPropertyName("exclusive")]
        bool Exclusive { get; }
        
        [JsonPropertyName("consumer_tag")]
        string ConsumerTag { get; }
        
        [JsonPropertyName("channel_details")]
        ChannelDetails ChannelDetails { get; }
        
        [JsonPropertyName("queue")]
        QueueConsumerDetails QueueConsumerDetails { get; }
    }
}