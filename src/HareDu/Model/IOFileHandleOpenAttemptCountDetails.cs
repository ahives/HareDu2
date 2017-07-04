namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface IOFileHandleOpenAttemptCountDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}