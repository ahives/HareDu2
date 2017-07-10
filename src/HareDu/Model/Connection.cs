namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface Connection
    {
        [JsonProperty("queue_index_journal_write_count")]
        long TotalQueueIndexJournalWrites { get; }

        [JsonProperty("queue_index_journal_write_count_details")]
        Rate RateOfQueueIndexJournalWrites { get; }


    }
}