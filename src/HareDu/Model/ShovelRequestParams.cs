namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class ShovelRequestParams
    {
        public ShovelRequestParams(
            AckMode acknowledgeMode,
            int reconnectDelay,
            ShovelProtocolType sourceProtocol,
            string sourceUri,
            string sourceQueue,
            string sourceExchange,
            string sourceExchangeRoutingKey,
            ulong sourcePrefetchCount,
            object sourceDeleteAfter,
            ShovelProtocolType destinationProtocol,
            string destinationUri,
            string destinationExchange,
            string destinationExchangeRoutingKey,
            string destinationQueue,
            bool destinationAddForwardHeaders,
            bool destinationAddTimestampHeader)
        {
            SourceProtocol = sourceProtocol;
            SourceUri = sourceUri;
            SourceQueue = sourceQueue;
            DestinationProtocol = destinationProtocol;
            DestinationUri = destinationUri;
            DestinationQueue = destinationQueue;
            ReconnectDelay = reconnectDelay;
            AcknowledgeMode = acknowledgeMode;
            SourceDeleteAfter = sourceDeleteAfter;
            SourcePrefetchCount = sourcePrefetchCount;
            SourceExchange = sourceExchange;
            SourceExchangeRoutingKey = sourceExchangeRoutingKey;
            DestinationExchange = destinationExchange;
            DestinationExchangeRoutingKey = destinationExchangeRoutingKey;
            // DestinationPublishProperties = destinationPublishProperties;
            DestinationAddForwardHeaders = destinationAddForwardHeaders;
            DestinationAddTimestampHeader = destinationAddTimestampHeader;
        }

        [JsonPropertyName("src-protocol")]
        public ShovelProtocolType SourceProtocol { get; set; }
        
        [JsonPropertyName("src-uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SourceUri { get; set; }
        
        [JsonPropertyName("src-queue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SourceQueue { get; set; }

        [JsonPropertyName("dest-protocol")]
        public ShovelProtocolType DestinationProtocol { get; set; }
        
        [JsonPropertyName("dest-uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DestinationUri { get; set; }
        
        [JsonPropertyName("dest-queue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DestinationQueue { get; set; }
        
        [JsonPropertyName("reconnect-delay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ReconnectDelay { get; set; }
        
        [JsonPropertyName("ack-mode")]
        public AckMode AcknowledgeMode { get; set; }
        
        [JsonPropertyName("src-delete-after")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object SourceDeleteAfter { get; set; }
        
        [JsonPropertyName("src-prefetch-count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ulong SourcePrefetchCount { get; set; }
        
        [JsonPropertyName("src-exchange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SourceExchange { get; set; }
        
        [JsonPropertyName("src-exchange-key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SourceExchangeRoutingKey { get; set; }
        
        [JsonPropertyName("dest-exchange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DestinationExchange { get; set; }
        
        [JsonPropertyName("dest-exchange-key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DestinationExchangeRoutingKey { get; set; }
        
        // [JsonPropertyName("dest-publish-properties")]
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // public string DestinationPublishProperties { get; set; }
        
        [JsonPropertyName("dest-add-forward-headers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool DestinationAddForwardHeaders { get; set; }
        
        [JsonPropertyName("dest-add-timestamp-header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool DestinationAddTimestampHeader { get; set; }
    }
}