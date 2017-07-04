namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MnesiaRamTransactionCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}