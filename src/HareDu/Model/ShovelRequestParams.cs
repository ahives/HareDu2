namespace HareDu.Model
{
    using System.Text.Json.Serialization;
    using Serialization.Converters;

    public interface ShovelRequestParams
    {
        [JsonPropertyName("src-protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        ShovelProtocolType SourceProtocol { get; }
        
        [JsonPropertyName("src-uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string SourceUri { get; }
        
        [JsonPropertyName("src-queue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string SourceQueue { get; }

        [JsonPropertyName("dest-protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        ShovelProtocolType DestinationProtocol { get; }
        
        [JsonPropertyName("dest-uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string DestinationUri { get; }
        
        [JsonPropertyName("dest-queue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string DestinationQueue { get; }
        
        [JsonPropertyName("reconnect-delay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        int ReconnectDelay { get; }
        
        [JsonPropertyName("ack-mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(AckModeEnumConverter))]
        AckMode AcknowledgeMode { get; }
        
        [JsonPropertyName("src-delete-after")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        object SourceDeleteAfter { get; }
        
        [JsonPropertyName("src-prefetch-count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        ulong SourcePrefetchCount { get; }
        
        [JsonPropertyName("src-exchange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SourceExchange { get; }
        
        [JsonPropertyName("src-exchange-key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string SourceExchangeRoutingKey { get; }
        
        [JsonPropertyName("dest-exchange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string DestinationExchange { get; }
        
        [JsonPropertyName("dest-exchange-key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string DestinationExchangeKey { get; }
        
        [JsonPropertyName("dest-publish-properties")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string DestinationPublishProperties { get; }
        
        [JsonPropertyName("dest-add-forward-headers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        bool DestinationAddForwardHeaders { get; }
        
        [JsonPropertyName("dest-add-timestamp-header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        bool DestinationAddTimestampHeader { get; }
    }
}