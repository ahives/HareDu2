namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalGarbageCollectionMetrics :
        GarbageCollectionMetrics
    {
        public InternalGarbageCollectionMetrics(GarbageCollectionMetricsImpl obj)
        {
            ConnectionsClosed = obj.ConnectionsClosed;
            ChannelsClosed = obj.ChannelsClosed;
            ConsumersDeleted = obj.ConsumersDeleted;
            ExchangesDeleted = obj.ExchangesDeleted;
            QueuesDeleted = obj.QueuesDeleted;
            VirtualHostsDeleted = obj.VirtualHostsDeleted;
            NodesDeleted = obj.NodesDeleted;
            ChannelConsumersDeleted = obj.ChannelConsumersDeleted;
        }

        public ulong ConnectionsClosed { get; }
        public ulong ChannelsClosed { get; }
        public ulong ConsumersDeleted { get; }
        public ulong ExchangesDeleted { get; }
        public ulong QueuesDeleted { get; }
        public ulong VirtualHostsDeleted { get; }
        public ulong NodesDeleted { get; }
        public ulong ChannelConsumersDeleted { get; }
    }
}