namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface NodeSnapshot :
        Snapshot
    {
        OperatingSystemSnapshot OS { get; }

        string RatesMode { get; }

        long Uptime { get; }
        
        long InterNodeHeartbeat { get; }

        string Identifier { get; }
        
        string ClusterIdentifier { get; }

        string Type { get; }

        bool IsRunning { get; }
        
        ulong AvailableCoresDetected { get; }

        IReadOnlyList<string> NetworkPartitions { get; }
        
        DiskSnapshot Disk { get; }
        
        BrokerRuntimeSnapshot Runtime { get; }
        
        MemorySnapshot Memory { get; }

        ContextSwitchingDetails ContextSwitching { get; }
    }
}