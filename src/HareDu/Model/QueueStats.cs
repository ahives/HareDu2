namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface QueueStats
    {
        [JsonProperty("messages_ready")]
        long TotalMessagesReady { get; }

        [JsonProperty("messages_ready_details")]
        Rate RateOfMessagesReady { get; }
        
        [JsonProperty("messages_unacknowledged")]
        long TotalMessagesUnacknowledged { get; }

        [JsonProperty("messages_unacknowledged_details")]
        Rate RateOfMessagesUnacknowledged { get; }
        
        [JsonProperty("messages")]
        long TotalMessages { get; }

        [JsonProperty("messages_details")]
        Rate RateOfMessages { get; }
    }
}