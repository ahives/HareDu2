namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using Snapshotting.Model;

    public class FakeNodeSnapshot1 :
        NodeSnapshot
    {
        public FakeNodeSnapshot1(ulong availableSockets, ulong usedSockets, decimal socketUsageRate)
        {
            OS = new OperatingSystemSnapshotImpl(availableSockets, usedSockets, socketUsageRate);
        }

        public OperatingSystemSnapshot OS { get; }
        public string RatesMode { get; }
        public long Uptime { get; }
        public long InterNodeHeartbeat { get; }
        public string Identifier { get; }
        public string ClusterIdentifier { get; }
        public string Type { get; }
        public bool IsRunning { get; }
        public ulong AvailableCoresDetected { get; }
        public IReadOnlyList<string> NetworkPartitions { get; }
        public DiskSnapshot Disk { get; }
        public IO IO { get; }
        public BrokerRuntimeSnapshot Runtime { get; }
        public MemorySnapshot Memory { get; }
        public ContextSwitchingDetails ContextSwitching { get; }

        
        class OperatingSystemSnapshotImpl :
            OperatingSystemSnapshot
        {
            public OperatingSystemSnapshotImpl(ulong available, ulong used, decimal usageRate)
            {
                SocketDescriptors = new SocketDescriptorChurnMetricsImpl(available, used, usageRate);
            }

            public string NodeIdentifier { get; }
            public string ProcessId { get; }
            public FileDescriptorChurnMetrics FileDescriptors { get; }
            public SocketDescriptorChurnMetrics SocketDescriptors { get; }

            
            class SocketDescriptorChurnMetricsImpl :
                SocketDescriptorChurnMetrics
            {
                public SocketDescriptorChurnMetricsImpl(ulong available, ulong used, decimal usageRate)
                {
                    Available = available;
                    Used = used;
                    UsageRate = usageRate;
                }

                public ulong Available { get; }
                public ulong Used { get; }
                public decimal UsageRate { get; }
            }
        }
    }
}