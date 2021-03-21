namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface VirtualHostDescription
    {
        [JsonPropertyName("rabbit_version")]
        string Version { get; }
        
        [JsonPropertyName("policies")]
        IList<PolicyDescription> Policies { get; }
        
        [JsonPropertyName("queues")]
        IList<QueueDescription> Queues { get; }
        
        [JsonPropertyName("exchanges")]
        IList<ExchangeDescription> Exchanges { get; }
        
        [JsonPropertyName("bindings")]
        IList<BindingDescription> Bindings { get; }
    }
}