namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface QueueIndexWriteCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}