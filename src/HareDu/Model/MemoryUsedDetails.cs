namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MemoryUsedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}