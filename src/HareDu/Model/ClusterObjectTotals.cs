namespace HareDu.Model
{
    public interface ClusterObjectTotals
    {
        ulong TotalConsumers { get; }
        
        ulong TotalQueues { get; }
        
        ulong TotalExchanges { get; }
        
        ulong TotalConnections { get; }
        
        ulong TotalChannels { get; }
    }
}