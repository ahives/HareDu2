namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface DiskFreeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}