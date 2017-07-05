namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface Listener
    {
        [JsonProperty("node")]
        string Node { get; }
        
        [JsonProperty("protocol")]
        string Protocol { get; }
        
        [JsonProperty("ip_address")]
        string IPAddress { get; }
        
        [JsonProperty("port")]
        string Port { get; }
        
        [JsonProperty("socket_opts")]
        IEnumerable<SocketOptions> SocketOptions { get; }
    }
}