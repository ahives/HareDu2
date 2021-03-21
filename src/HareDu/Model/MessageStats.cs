namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface MessageStats
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
        ulong TotalUnroutableMessages { get; }

        [JsonPropertyName("return_unroutable_details")]
        Rate UnroutableMessagesDetails { get; }
        
        [JsonPropertyName("disk_reads")]
        ulong TotalDiskReads { get; }

        [JsonPropertyName("disk_reads_details")]
        Rate DiskReadDetails { get; }
        
        [JsonPropertyName("disk_writes")]
        ulong TotalDiskWrites { get; }

        [JsonPropertyName("disk_writes_details")]
        Rate DiskWriteDetails { get; }
        
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
        
        [JsonPropertyName("redeliver")]
        ulong TotalMessagesRedelivered { get; }

        [JsonPropertyName("redeliver_details")]
        Rate MessagesRedeliveredDetails { get; }
        
        [JsonPropertyName("ack")]
        ulong TotalMessagesAcknowledged { get; }

        [JsonPropertyName("ack_details")]
        Rate MessagesAcknowledgedDetails { get; }
        
        [JsonPropertyName("deliver_get")]
        ulong TotalMessageDeliveryGets { get; }

        [JsonPropertyName("deliver_get_details")]
        Rate MessageDeliveryGetDetails { get; }
    }
}