namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalServerInfo :
        ServerInfo
    {
        public InternalServerInfo(ServerInfoImpl obj)
        {
            RabbitMqVersion = obj.RabbitMqVersion;
            Users = MapUsers(obj.Users);
            VirtualHosts = MapVirtualHosts(obj.VirtualHosts);
            Permissions = MapPermissions(obj.Permissions);
            Policies = MapPolicies(obj.Policies);
            Parameters = MapParameters(obj.Parameters);
            GlobalParameters = MapGlobalParameters(obj.GlobalParameters);
            Queues = MapQueues(obj.Queues);
            Exchanges = MapExchanges(obj.Exchanges);
            Bindings = MapBindings(obj.Bindings);
            TopicPermissions = MapTopicPermissions(obj.TopicPermissions);
        }

        public string RabbitMqVersion { get; }
        public IList<UserInfo> Users { get; }
        public IList<VirtualHostInfo> VirtualHosts { get; }
        public IList<UserPermissionsInfo> Permissions { get; }
        public IList<PolicyInfo> Policies { get; }
        public IList<ScopedParameterInfo> Parameters { get; }
        public IList<GlobalParameterInfo> GlobalParameters { get; }
        public IList<QueueInfo> Queues { get; }
        public IList<ExchangeInfo> Exchanges { get; }
        public IList<BindingInfo> Bindings { get; }
        public IList<TopicPermissionsInfo> TopicPermissions { get; }

        IList<UserInfo> MapUsers(IList<UserInfoImpl> users)
        {
            if (users.IsNull())
                return default;

            var list = new List<UserInfo>();
            foreach (var userInfo in users)
                list.Add(new InternalUserInfo(userInfo));

            return list;
        }

        IList<VirtualHostInfo> MapVirtualHosts(IList<VirtualHostInfoImpl> vhosts)
        {
            if (vhosts.IsNull())
                return default;

            var list = new List<VirtualHostInfo>();
            foreach (var vhost in vhosts)
                list.Add(new InternalVirtualHostInfo(vhost));

            return list;
        }

        IList<UserPermissionsInfo> MapPermissions(IList<UserPermissionsInfoImpl> permissions)
        {
            if (permissions.IsNull())
                return default;

            var list = new List<UserPermissionsInfo>();
            foreach (var permission in permissions)
                list.Add(new InternalUserPermissionsInfo(permission));

            return list;
        }

        IList<PolicyInfo> MapPolicies(IList<PolicyInfoImpl> policies)
        {
            if (policies.IsNull())
                return default;

            var list = new List<PolicyInfo>();
            foreach (var policy in policies)
                list.Add(new InternalPolicyInfo(policy));

            return list;
        }

        IList<ScopedParameterInfo> MapParameters(IList<ScopedParameterInfoImpl> parameters)
        {
            if (parameters.IsNull())
                return default;

            var list = new List<ScopedParameterInfo>();
            foreach (var parameter in parameters)
                list.Add(new InternalScopedParameterInfo(parameter));

            return list;
        }

        IList<GlobalParameterInfo> MapGlobalParameters(IList<GlobalParameterInfoImpl> parameters)
        {
            if (parameters.IsNull())
                return default;

            var list = new List<GlobalParameterInfo>();
            foreach (var parameter in parameters)
                list.Add(new InternalGlobalParameterInfo(parameter));

            return list;
        }

        IList<QueueInfo> MapQueues(IList<QueueInfoImpl> queues)
        {
            if (queues.IsNull())
                return default;

            var list = new List<QueueInfo>();
            foreach (var queue in queues)
                list.Add(new InternalQueueInfo(queue));

            return list;
        }

        IList<ExchangeInfo> MapExchanges(IList<ExchangeInfoImpl> exchanges)
        {
            if (exchanges.IsNull())
                return default;

            var list = new List<ExchangeInfo>();
            foreach (var exchange in exchanges)
                list.Add(new InternalExchangeInfo(exchange));

            return list;
        }

        IList<BindingInfo> MapBindings(IList<BindingInfoImpl> bindings)
        {
            if (bindings.IsNull())
                return default;

            var list = new List<BindingInfo>();
            foreach (var binding in bindings)
                list.Add(new InternalBindingInfo(binding));

            return list;
        }

        IList<TopicPermissionsInfo> MapTopicPermissions(IList<TopicPermissionsInfoImpl> permissions)
        {
            if (permissions.IsNull())
                return default;

            var list = new List<TopicPermissionsInfo>();
            foreach (var permission in permissions)
                list.Add(new InternalTopicPermissionsInfo(permission));

            return list;
        }
    }
}