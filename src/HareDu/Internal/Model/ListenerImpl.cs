namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;
    using Serialization.Converters;

    class ListenerImpl
    {
        [JsonPropertyName("node")]
        public string Node { get; set; }
        
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        
        [JsonPropertyName("ip_address")]
        public string IPAddress { get; set; }
        
        [JsonPropertyName("port")]
        public string Port { get; set; }
        
        [JsonPropertyName("socket_opts")]
        [JsonConverter(typeof(InconsistentObjectDataConverter))]
        public SocketOptionsImpl SocketOptions { get; set; }
    }
}