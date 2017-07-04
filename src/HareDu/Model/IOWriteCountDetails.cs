namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOWriteCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}