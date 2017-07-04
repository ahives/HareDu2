namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOSeekAvgTimeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}