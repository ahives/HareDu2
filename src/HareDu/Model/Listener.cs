namespace HareDu.Model
{
    using System.Text.Json.Serialization;
    using Serialization.Converters;

    public interface Listener
    {
        [JsonPropertyName("node")]
        string Node { get; }
        
        [JsonPropertyName("protocol")]
        string Protocol { get; }
        
        [JsonPropertyName("ip_address")]
        string IPAddress { get; }
        
        [JsonPropertyName("port")]
        string Port { get; }
        
        [JsonPropertyName("socket_opts")]
        [JsonConverter(typeof(InconsistentObjectDataConverter))]
        SocketOptions SocketOptions { get; }
    }
}