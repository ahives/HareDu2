namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalObjectTotals :
        ClusterObjectTotals
    {
        public InternalObjectTotals(ClusterObjectTotalsImpl obj)
        {
            TotalConsumers = obj.TotalConsumers;
            TotalQueues = obj.TotalQueues;
            TotalExchanges = obj.TotalExchanges;
            TotalConnections = obj.TotalConnections;
            TotalChannels = obj.TotalChannels;
        }

        public ulong TotalConsumers { get; }
        public ulong TotalQueues { get; }
        public ulong TotalExchanges { get; }
        public ulong TotalConnections { get; }
        public ulong TotalChannels { get; }
    }
}