namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOWriteBytesDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}