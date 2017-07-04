namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface GarbageCollectionNumberDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}