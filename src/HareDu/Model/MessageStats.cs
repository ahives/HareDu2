namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MessageStats
    {
        [JsonProperty("publish")]
        long TotalMessagesPublished { get; }

        [JsonProperty("publish_details")]
        Rate RateOfMessagesPublished { get; }
        
        [JsonProperty("confirm")]
        long TotalMessagesConfirmed { get; }

        [JsonProperty("confirm_details")]
        Rate RateOfMessagesConfirmed { get; }
        
        [JsonProperty("return_unroutable")]
        long TotalUnroutableMessagesReturned { get; }

        [JsonProperty("return_unroutable_details")]
        Rate RateOfUnroutableMessagesReturned { get; }
        
        [JsonProperty("disk_reads")]
        long TotalDiskReads { get; }

        [JsonProperty("disk_reads_details")]
        Rate RateOfDiskReads { get; }
        
        [JsonProperty("disk_writes")]
        long TotalDiskWrites { get; }

        [JsonProperty("disk_writes_details")]
        Rate RateOfDiskWrites { get; }
        
        [JsonProperty("get")]
        long TotalMessageGets { get; }

        [JsonProperty("get_details")]
        Rate RateOfMessageGets { get; }
        
        [JsonProperty("get_no_ack")]
        long TotalMessageGetsWithoutAck { get; }

        [JsonProperty("get_no_ack_details")]
        Rate RateOfMessageGetsWithoutAck { get; }
        
        [JsonProperty("deliver")]
        long TotalMessageDelivered { get; }

        [JsonProperty("deliver_details")]
        Rate RateOfMessageDelivered { get; }
        
        [JsonProperty("deliver_no_ack")]
        long TotalMessageDeliveredWithoutAck { get; }

        [JsonProperty("deliver_no_ack_details")]
        Rate RateOfMessageDeliveredWithoutAck { get; }
        
        [JsonProperty("redeliver")]
        long TotalMessagesRedelivered { get; }

        [JsonProperty("redeliver_details")]
        Rate RateOfMessagesRedelivered { get; }
        
        [JsonProperty("ack")]
        long TotalMessagesAcknowledged { get; }

        [JsonProperty("ack_details")]
        Rate RateOfMessagesAcknowledged { get; }
        
        [JsonProperty("deliver_get")]
        long TotalMessageDeliveryGets { get; }

        [JsonProperty("deliver_get_details")]
        Rate RateOfMessageDeliveryGets { get; }
    }
}