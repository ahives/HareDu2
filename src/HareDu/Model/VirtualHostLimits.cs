namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface VirtualHostLimits
    {
        [JsonProperty("vhost")]
        string VirtualHostName { get; }
        
        [JsonProperty("value")]
        IDictionary<string, string> Limits { get; }
    }
}