namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ContextSwitchesDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}