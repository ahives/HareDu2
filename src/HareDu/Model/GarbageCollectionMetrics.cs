namespace HareDu.Model
{
    public interface GarbageCollectionMetrics
    {
        ulong ConnectionsClosed { get; }

        ulong ChannelsClosed { get; }

        ulong ConsumersDeleted { get; }
        
        ulong ExchangesDeleted { get; }

        ulong QueuesDeleted { get; }

        ulong VirtualHostsDeleted { get; }

        ulong NodesDeleted { get; }

        ulong ChannelConsumersDeleted { get; }
    }
}