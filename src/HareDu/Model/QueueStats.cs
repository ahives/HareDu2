namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface QueueStats
    {
        [JsonPropertyName("messages_ready")]
        ulong TotalMessagesReadyForDelivery { get; }

        [JsonPropertyName("messages_ready_details")]
        Rate MessagesReadyForDeliveryDetails { get; }
        
        [JsonPropertyName("messages_unacknowledged")]
        ulong TotalUnacknowledgedDeliveredMessages { get; }

        [JsonPropertyName("messages_unacknowledged_details")]
        Rate UnacknowledgedDeliveredMessagesDetails { get; }
        
        [JsonPropertyName("messages")]
        ulong TotalMessages { get; }

        [JsonPropertyName("messages_details")]
        Rate MessageDetails { get; }
    }
}