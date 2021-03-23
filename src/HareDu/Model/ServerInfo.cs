namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface ServerInfo
    {
        string RabbitMqVersion { get; }
        
        IList<UserInfo> Users { get; }
        
        IList<VirtualHostInfo> VirtualHosts { get; }
        
        IList<UserPermissionsInfo> Permissions { get; }
        
        IList<PolicyInfo> Policies { get; }
        
        IList<ScopedParameterInfo> Parameters { get; }
        
        IList<GlobalParameterInfo> GlobalParameters { get; }
        
        IList<QueueInfo> Queues { get; }
        
        IList<ExchangeInfo> Exchanges { get; }
        
        IList<BindingInfo> Bindings { get; }
        
        IList<TopicPermissionsInfo> TopicPermissions { get; }
    }
}