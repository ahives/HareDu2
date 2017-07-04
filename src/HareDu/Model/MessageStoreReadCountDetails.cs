namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MessageStoreReadCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}