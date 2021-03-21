namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface SocketOptions
    {
        [JsonPropertyName("backlog")]
        long Backlog { get; }
        
        [JsonPropertyName("nodelay")]
        bool NoDelay { get; }
        
        [JsonPropertyName("exit_on_close")]
        bool ExitOnClose { get; }
    }
}