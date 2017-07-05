namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface SocketOptions
    {
        [JsonProperty("backlog")]
        long Backlog { get; }
        
        [JsonProperty("nodelay")]
        bool NoDelay { get; }
        
        [JsonProperty("exit_on_close")]
        bool ExitOnClose { get; }
    }
}