namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeBrokerRuntimeSnapshot1 :
        BrokerRuntimeSnapshot
    {
        public FakeBrokerRuntimeSnapshot1(ulong limit, ulong used, decimal usageRate)
        {
            Processes = new RuntimeProcessChurnMetricsImpl(limit, used, usageRate);
        }

        public string Identifier { get; }
        public string ClusterIdentifier { get; }
        public string Version { get; }
        public RuntimeProcessChurnMetrics Processes { get; }
        public RuntimeDatabase Database { get; }
        public GarbageCollection GC { get; }


        class RuntimeProcessChurnMetricsImpl :
            RuntimeProcessChurnMetrics
        {
            public RuntimeProcessChurnMetricsImpl(ulong limit, ulong used, decimal usageRate)
            {
                Limit = limit;
                Used = used;
                UsageRate = usageRate;
            }

            public ulong Limit { get; }
            public ulong Used { get; }
            public decimal UsageRate { get; }
        }
    }
}