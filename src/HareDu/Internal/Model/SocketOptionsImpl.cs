namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class SocketOptionsImpl
    {
        [JsonPropertyName("backlog")]
        public long Backlog { get; set; }
        
        [JsonPropertyName("nodelay")]
        public bool NoDelay { get; set; }
        
        [JsonPropertyName("exit_on_close")]
        public bool ExitOnClose { get; set; }
    }
}