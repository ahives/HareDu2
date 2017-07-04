namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOWriteAvgTimeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}