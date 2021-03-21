namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface ServerInfo
    {
        [JsonPropertyName("rabbit_version")]
        string RabbitMqVersion { get; }
        
        [JsonPropertyName("users")]
        IList<UserInfo> Users { get; }
        
        [JsonPropertyName("vhosts")]
        IList<VirtualHostInfo> VirtualHosts { get; }
        
        [JsonPropertyName("permissions")]
        IList<UserPermissionsInfo> Permissions { get; }
        
        [JsonPropertyName("policies")]
        IList<PolicyInfo> Policies { get; }
        
        [JsonPropertyName("parameters")]
        IList<ScopedParameterInfo> Parameters { get; }
        
        [JsonPropertyName("global_parameters")]
        IList<GlobalParameterInfo> GlobalParameters { get; }
        
        [JsonPropertyName("queues")]
        IList<QueueInfo> Queues { get; }
        
        [JsonPropertyName("exchanges")]
        IList<ExchangeInfo> Exchanges { get; }
        
        [JsonPropertyName("bindings")]
        IList<BindingInfo> Bindings { get; }
        
        [JsonPropertyName("topic_permissions")]
        IList<TopicPermissionsInfo> TopicPermissions { get; }
    }
}