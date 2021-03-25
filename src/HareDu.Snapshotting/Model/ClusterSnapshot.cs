namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface ClusterSnapshot :
        Snapshot
    {
        string BrokerVersion { get; }
        
        string ClusterName { get; }
        
        IReadOnlyList<NodeSnapshot> Nodes { get; }
    }
}