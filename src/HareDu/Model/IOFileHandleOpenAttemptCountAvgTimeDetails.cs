namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOFileHandleOpenAttemptCountAvgTimeDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}