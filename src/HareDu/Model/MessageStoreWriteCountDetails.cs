namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MessageStoreWriteCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}