namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ChannelOperationStats
    {
        [JsonPropertyName("publish")]
        ulong TotalMessagesPublished { get; }

        [JsonPropertyName("publish_details")]
        Rate MessagesPublishedDetails { get; }
        
        [JsonPropertyName("confirm")]
        ulong TotalMessagesConfirmed { get; }

        [JsonPropertyName("confirm_details")]
        Rate MessagesConfirmedDetails { get; }
        
        [JsonPropertyName("return_unroutable")]
        ulong TotalMessagesNotRouted { get; }

        [JsonPropertyName("return_unroutable_details")]
        Rate MessagesNotRoutedDetails { get; }
        
        [JsonPropertyName("get")]
        ulong TotalMessageGets { get; }

        [JsonPropertyName("get_details")]
        Rate MessageGetDetails { get; }
        
        [JsonPropertyName("get_no_ack")]
        ulong TotalMessageGetsWithoutAck { get; }

        [JsonPropertyName("get_no_ack_details")]
        Rate MessageGetsWithoutAckDetails { get; }
        
        [JsonPropertyName("deliver")]
        ulong TotalMessagesDelivered { get; }

        [JsonPropertyName("deliver_details")]
        Rate MessageDeliveryDetails { get; }
        
        [JsonPropertyName("deliver_no_ack")]
        ulong TotalMessageDeliveredWithoutAck { get; }

        [JsonPropertyName("deliver_no_ack_details")]
        Rate MessagesDeliveredWithoutAckDetails { get; }
        
        [JsonPropertyName("deliver_get")]
        ulong TotalMessageDeliveryGets { get; }

        [JsonPropertyName("deliver_get_details")]
        Rate MessageDeliveryGetDetails { get; }
        
        [JsonPropertyName("redeliver")]
        ulong TotalMessagesRedelivered { get; }

        [JsonPropertyName("redeliver_details")]
        Rate MessagesRedeliveredDetails { get; }
        
        [JsonPropertyName("ack")]
        ulong TotalMessagesAcknowledged { get; }

        [JsonPropertyName("ack_details")]
        Rate MessagesAcknowledgedDetails { get; }
    }
}