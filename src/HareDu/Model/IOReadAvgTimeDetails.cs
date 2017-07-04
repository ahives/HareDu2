namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOReadAvgTimeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}