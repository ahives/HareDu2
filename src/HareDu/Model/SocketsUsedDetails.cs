namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface SocketsUsedDetails
    {
        [JsonProperty("rate")]
        decimal Rate { get; }
    }
}