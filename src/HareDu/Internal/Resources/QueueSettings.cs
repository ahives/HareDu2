namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface QueueSettings
    {
        [JsonProperty("node")]
        string Node { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
                
        [JsonProperty("arguments", Required = Required.Default)]
        IDictionary<string, object> Arguments { get; }
    }
}