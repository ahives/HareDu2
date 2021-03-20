namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ShovelRequestParams
    {
        [JsonProperty("src-protocol")]
        string SourceProtocol { get; }
        
        [JsonProperty("src-uri")]
        string SourceUri { get; }
        
        [JsonProperty("src-queue")]
        string SourceQueue { get; }

        [JsonProperty("dest-protocol")]
        string DestinationProtocol { get; }
        
        [JsonProperty("dest-uri")]
        string DestinationUri { get; }
        
        [JsonProperty("dest-queue")]
        string DestinationQueue { get; }
        
        [JsonProperty("reconnect-delay")]
        int ReconnectDelay { get; }
        
        [JsonProperty("ack-mode")]
        string AcknowledgeMode { get; }
        
        [JsonProperty("src-delete-after")]
        object SourceDeleteAfter { get; }
        
        [JsonProperty("src-prefetch-count")]
        ulong SourcePrefetchCount { get; }
        
        [JsonProperty("src-exchange")]
        string SourceExchange { get; }
        
        [JsonProperty("src-exchange-key")]
        string SourceExchangeRoutingKey { get; }
        
        [JsonProperty("dest-exchange")]
        string DestinationExchange { get; }
        
        [JsonProperty("dest-exchange-key")]
        string DestinationExchangeKey { get; }
        
        [JsonProperty("dest-publish-properties")]
        string DestinationPublishProperties { get; }
        
        [JsonProperty("dest-add-forward-headers")]
        bool DestinationAddForwardHeaders { get; }
        
        [JsonProperty("dest-add-timestamp-header")]
        bool DestinationAddTimestampHeader { get; }
    }
}