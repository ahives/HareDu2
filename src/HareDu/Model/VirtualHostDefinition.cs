namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface VirtualHostDefinition
    {
        [JsonProperty("rabbit_version")]
        string Version { get; }
        
        [JsonProperty("policies")]
        IEnumerable<PolicySummary> Policies { get; }
        
        [JsonProperty("queues")]
        IEnumerable<QueueSummary> Queues { get; }
        
        [JsonProperty("exchanges")]
        IEnumerable<ExchangeSummary> Exchanges { get; }
        
        [JsonProperty("bindings")]
        IEnumerable<BindingSummary> Bindings { get; }
    }
}