namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MnesiaDiskTransactionCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}