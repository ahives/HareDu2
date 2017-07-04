namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface GarbageCollectionBytesReclaimedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}