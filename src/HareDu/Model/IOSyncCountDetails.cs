namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOSyncCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}