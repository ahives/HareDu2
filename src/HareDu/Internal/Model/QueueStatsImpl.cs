namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class QueueStatsImpl
    {
        [JsonPropertyName("messages_ready")]
        public ulong TotalMessagesReadyForDelivery { get; set; }

        [JsonPropertyName("messages_ready_details")]
        public RateImpl MessagesReadyForDeliveryDetails { get; set; }
        
        [JsonPropertyName("messages_unacknowledged")]
        public ulong TotalUnacknowledgedDeliveredMessages { get; set; }

        [JsonPropertyName("messages_unacknowledged_details")]
        public RateImpl UnacknowledgedDeliveredMessagesDetails { get; set; }
        
        [JsonPropertyName("messages")]
        public ulong TotalMessages { get; set; }

        [JsonPropertyName("messages_details")]
        public RateImpl MessageDetails { get; set; }
    }
}