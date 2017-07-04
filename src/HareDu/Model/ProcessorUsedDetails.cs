namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ProcessorUsedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}