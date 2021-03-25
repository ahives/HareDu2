namespace HareDu.Snapshotting.Model
{
    public interface BrokerRuntimeSnapshot :
        Snapshot
    {
        string Identifier { get; }
        
        string ClusterIdentifier { get; }
        
        string Version { get; }

        RuntimeProcessChurnMetrics Processes { get; }
        
        RuntimeDatabase Database { get; }
        
        GarbageCollection GC { get; }
    }
}