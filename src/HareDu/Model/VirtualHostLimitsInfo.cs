namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface VirtualHostLimitsInfo
    {
        [JsonPropertyName("vhost")]
        string VirtualHostName { get; }
        
        [JsonPropertyName("value")]
        IDictionary<string, ulong> Limits { get; }
    }
}