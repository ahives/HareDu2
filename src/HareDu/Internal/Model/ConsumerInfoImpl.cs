namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class ConsumerInfoImpl
    {
        [JsonPropertyName("prefetch_count")]
        public ulong PreFetchCount { get; set; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
        
        [JsonPropertyName("ack_required")]
        public bool AcknowledgementRequired { get; set; }
        
        [JsonPropertyName("exclusive")]
        public bool Exclusive { get; set; }
        
        [JsonPropertyName("consumer_tag")]
        public string ConsumerTag { get; set; }
        
        [JsonPropertyName("channel_details")]
        public ChannelDetailsImpl ChannelDetails { get; set; }
        
        [JsonPropertyName("queue")]
        public QueueConsumerDetailsImpl QueueConsumerDetails { get; set; }
    }
}