namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface QueueIndexReadCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}