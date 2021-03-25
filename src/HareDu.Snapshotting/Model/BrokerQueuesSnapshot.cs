namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface BrokerQueuesSnapshot :
        Snapshot
    {
        string ClusterName { get; }
        
        BrokerQueueChurnMetrics Churn { get; }
        
        IReadOnlyList<QueueSnapshot> Queues { get; }
    }
}