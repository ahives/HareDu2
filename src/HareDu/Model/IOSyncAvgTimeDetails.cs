namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOSyncAvgTimeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}