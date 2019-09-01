namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeBrokerRuntimeSnapshot1 :
        BrokerRuntimeSnapshot
    {
        public FakeBrokerRuntimeSnapshot1(long availableCores, long limit, long used, decimal usageRate)
        {
            AvailableCores = availableCores;
            Processes = new RuntimeProcessChurnMetricsImpl(limit, used, usageRate);
        }

        public string Version { get; }
        public long AvailableCores { get; }
        public RuntimeProcessChurnMetrics Processes { get; }

        
        class RuntimeProcessChurnMetricsImpl :
            RuntimeProcessChurnMetrics
        {
            public RuntimeProcessChurnMetricsImpl(long limit, long used, decimal usageRate)
            {
                Limit = limit;
                Used = used;
                UsageRate = usageRate;
            }

            public long Limit { get; }
            public long Used { get; }
            public decimal UsageRate { get; }
        }
    }
}