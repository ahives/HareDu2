namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface BrokerConnectivitySnapshot :
        Snapshot
    {
        string BrokerVersion { get; }
        
        string ClusterName { get; }
        
        ChurnMetrics ChannelsClosed { get; }

        ChurnMetrics ChannelsCreated { get; }

        ChurnMetrics ConnectionsClosed { get; }

        ChurnMetrics ConnectionsCreated { get; }
        
        IReadOnlyList<ConnectionSnapshot> Connections { get; }
    }
}