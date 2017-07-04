namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOReadBytesDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}