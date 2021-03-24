namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalSystemOverviewInfo :
        SystemOverviewInfo
    {
        public InternalSystemOverviewInfo(SystemOverviewInfoImpl obj)
        {
            ManagementVersion = obj.ManagementVersion;
            RatesMode = obj.RatesMode;
            SampleRetentionPolicies = obj.SampleRetentionPolicies.IsNotNull()
                ? new InternalSampleRetentionPolicies(obj.SampleRetentionPolicies)
                : default;
            ExchangeTypes = MapExchangeTypes(obj.ExchangeTypes);
            ProductVersion = obj.ProductVersion;
            ProductName = obj.ProductName;
            RabbitMqVersion = obj.RabbitMqVersion;
            ClusterName = obj.ClusterName;
            ErlangVersion = obj.ErlangVersion;
            ErlangFullVersion = obj.ErlangFullVersion;
            DisableStats = obj.DisableStats;
            EnableQueueTotals = obj.EnableQueueTotals;
            MessageStats = obj.MessageStats.IsNotNull() ? new InternalMessageStats(obj.MessageStats) : default;
            ChurnRates = obj.ChurnRates.IsNotNull() ? new InternalChurnRates(obj.ChurnRates) : default;
            QueueStats = obj.QueueStats.IsNotNull() ? new InternalQueueStats(obj.QueueStats) : default;
            ObjectTotals = obj.ObjectTotals.IsNotNull() ? new InternalObjectTotals(obj.ObjectTotals) : default;
            StatsDatabaseEventQueue = obj.StatsDatabaseEventQueue;
            Node = obj.Node;
            Listeners = MapListeners(obj.Listeners);
            Contexts = MapNodeContexts(obj.Contexts);
        }

        public string ManagementVersion { get; }
        public string RatesMode { get; }
        public SampleRetentionPolicies SampleRetentionPolicies { get; }
        public IList<ExchangeType> ExchangeTypes { get; }
        public string ProductVersion { get; }
        public string ProductName { get; }
        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public string ErlangVersion { get; }
        public string ErlangFullVersion { get; }
        public bool DisableStats { get; }
        public bool EnableQueueTotals { get; }
        public MessageStats MessageStats { get; }
        public ChurnRates ChurnRates { get; }
        public QueueStats QueueStats { get; }
        public ClusterObjectTotals ObjectTotals { get; }
        public ulong StatsDatabaseEventQueue { get; }
        public string Node { get; }
        public IList<Listener> Listeners { get; }
        public IList<NodeContext> Contexts { get; }

        IList<ExchangeType> MapExchangeTypes(IList<ExchangeTypeImpl> exchangeTypes)
        {
            if (exchangeTypes.IsNull())
                return default;

            var list = new List<ExchangeType>();
            foreach (ExchangeTypeImpl item in exchangeTypes)
                list.Add(new InternalExchangeType(item));

            return list;
        }

        IList<Listener> MapListeners(IList<ListenerImpl> listeners)
        {
            if (listeners.IsNull())
                return default;

            var list = new List<Listener>();
            foreach (ListenerImpl item in listeners)
                list.Add(new InternalListener(item));

            return list;
        }

        IList<NodeContext> MapNodeContexts(IList<NodeContextImpl> nodeContexts)
        {
            if (nodeContexts.IsNull())
                return default;

            var list = new List<NodeContext>();
            foreach (NodeContextImpl item in nodeContexts)
                list.Add(new InternalNodeContext(item));

            return list;
        }
    }
}