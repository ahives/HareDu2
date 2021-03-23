namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class ServerInfoImpl
    {
        [JsonPropertyName("rabbit_version")]
        public string RabbitMqVersion { get; set; }
        
        [JsonPropertyName("users")]
        public IList<UserInfoImpl> Users { get; set; }
        
        [JsonPropertyName("vhosts")]
        public IList<VirtualHostInfoImpl> VirtualHosts { get; set; }
        
        [JsonPropertyName("permissions")]
        public IList<UserPermissionsInfoImpl> Permissions { get; set; }
        
        [JsonPropertyName("policies")]
        public IList<PolicyInfoImpl> Policies { get; set; }
        
        [JsonPropertyName("parameters")]
        public IList<ScopedParameterInfoImpl> Parameters { get; set; }
        
        [JsonPropertyName("global_parameters")]
        public IList<GlobalParameterInfoImpl> GlobalParameters { get; set; }
        
        [JsonPropertyName("queues")]
        public IList<QueueInfoImpl> Queues { get; set; }
        
        [JsonPropertyName("exchanges")]
        public IList<ExchangeInfoImpl> Exchanges { get; set; }
        
        [JsonPropertyName("bindings")]
        public IList<BindingInfoImpl> Bindings { get; set; }
        
        [JsonPropertyName("topic_permissions")]
        public IList<TopicPermissionsInfoImpl> TopicPermissions { get; set; }
    }
}