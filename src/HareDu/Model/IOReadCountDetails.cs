namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOReadCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}