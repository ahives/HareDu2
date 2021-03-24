namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class MessageStatsImpl
    {
        [JsonPropertyName("publish")]
        public ulong TotalMessagesPublished { get; set; }

        [JsonPropertyName("publish_details")]
        public RateImpl MessagesPublishedDetails { get; set; }
        
        [JsonPropertyName("confirm")]
        public ulong TotalMessagesConfirmed { get; set; }

        [JsonPropertyName("confirm_details")]
        public RateImpl MessagesConfirmedDetails { get; set; }
        
        [JsonPropertyName("return_unroutable")]
        public ulong TotalUnroutableMessages { get; set; }

        [JsonPropertyName("return_unroutable_details")]
        public RateImpl UnroutableMessagesDetails { get; set; }
        
        [JsonPropertyName("disk_reads")]
        public ulong TotalDiskReads { get; set; }

        [JsonPropertyName("disk_reads_details")]
        public RateImpl DiskReadDetails { get; set; }
        
        [JsonPropertyName("disk_writes")]
        public ulong TotalDiskWrites { get; set; }

        [JsonPropertyName("disk_writes_details")]
        public RateImpl DiskWriteDetails { get; set; }
        
        [JsonPropertyName("get")]
        public ulong TotalMessageGets { get; set; }

        [JsonPropertyName("get_details")]
        public RateImpl MessageGetDetails { get; set; }
        
        [JsonPropertyName("get_no_ack")]
        public ulong TotalMessageGetsWithoutAck { get; set; }

        [JsonPropertyName("get_no_ack_details")]
        public RateImpl MessageGetsWithoutAckDetails { get; set; }
        
        [JsonPropertyName("deliver")]
        public ulong TotalMessagesDelivered { get; set; }

        [JsonPropertyName("deliver_details")]
        public RateImpl MessageDeliveryDetails { get; set; }
        
        [JsonPropertyName("deliver_no_ack")]
        public ulong TotalMessageDeliveredWithoutAck { get; set; }

        [JsonPropertyName("deliver_no_ack_details")]
        public RateImpl MessagesDeliveredWithoutAckDetails { get; set; }
        
        [JsonPropertyName("redeliver")]
        public ulong TotalMessagesRedelivered { get; set; }

        [JsonPropertyName("redeliver_details")]
        public RateImpl MessagesRedeliveredDetails { get; set; }
        
        [JsonPropertyName("ack")]
        public ulong TotalMessagesAcknowledged { get; set; }

        [JsonPropertyName("ack_details")]
        public RateImpl MessagesAcknowledgedDetails { get; set; }
        
        [JsonPropertyName("deliver_get")]
        public ulong TotalMessageDeliveryGets { get; set; }

        [JsonPropertyName("deliver_get_details")]
        public RateImpl MessageDeliveryGetDetails { get; set; }
    }
}