namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface QueueIndexJournalWriteCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}