namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using Snapshotting.Model;

    public class FakeNodeSnapshot3 :
        NodeSnapshot
    {
        public FakeNodeSnapshot3(ulong availableCoresDetected)
        {
            AvailableCoresDetected = availableCoresDetected;
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
        public BrokerRuntimeSnapshot Runtime { get; }
        public MemorySnapshot Memory { get; }
        public ContextSwitchingDetails ContextSwitching { get; }
    }
}