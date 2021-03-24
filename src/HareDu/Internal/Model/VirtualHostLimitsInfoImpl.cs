namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class VirtualHostLimitsInfoImpl
    {
        [JsonPropertyName("vhost")]
        public string VirtualHostName { get; set; }
        
        [JsonPropertyName("value")]
        public IDictionary<string, ulong> Limits { get; set; }
    }
}