namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOSeekCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}