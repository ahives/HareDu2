namespace HareDu.Core.Model
{
    using Newtonsoft.Json;

    public interface QueueMessageStats
    {
        [JsonProperty("publish")]
        long TotalMessagesPublished { get; }

        [JsonProperty("publish_details")]
        MessagesPublishedDetails MessagesPublishedDetails { get; }
        
        [JsonProperty("get")]
        long TotalMessageGets { get; }

        [JsonProperty("get_details")]
        MessageGetDetails MessageGetDetails { get; }
        
        [JsonProperty("get_no_ack")]
        long TotalMessageGetsWithoutAck { get; }

        [JsonProperty("get_no_ack_details")]
        MessageGetsWithoutAckDetails MessageGetsWithoutAckDetails { get; }
        
        [JsonProperty("deliver")]
        long TotalMessagesDelivered { get; }

        [JsonProperty("deliver_details")]
        MessageDeliveryDetails MessageDeliveryDetails { get; }
        
        [JsonProperty("deliver_no_ack")]
        long TotalMessageDeliveredWithoutAck { get; }

        [JsonProperty("deliver_no_ack_details")]
        MessagesDeliveredWithoutAckDetails MessagesDeliveredWithoutAckDetails { get; }
        
        [JsonProperty("deliver_get")]
        long TotalMessageDeliveryGets { get; }

        [JsonProperty("deliver_get_details")]
        MessageDeliveryGetDetails MessageDeliveryGetDetails { get; }
        
        [JsonProperty("redeliver")]
        long TotalMessagesRedelivered { get; }

        [JsonProperty("redeliver_details")]
        MessagesRedeliveredDetails MessagesRedeliveredDetails { get; }
        
        [JsonProperty("ack")]
        long TotalMessagesAcknowledged { get; }

        [JsonProperty("ack_details")]
        MessagesAcknowledgedDetails MessagesAcknowledgedDetails { get; }
    }
}