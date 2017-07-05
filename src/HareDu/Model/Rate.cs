namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface Rate
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}