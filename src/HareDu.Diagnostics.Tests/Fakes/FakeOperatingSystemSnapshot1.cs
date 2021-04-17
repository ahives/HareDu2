namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeOperatingSystemSnapshot1 :
        OperatingSystemSnapshot
    {
        public FakeOperatingSystemSnapshot1(ulong available, ulong used)
        {
            FileDescriptors = new FileDescriptorChurnMetricsImpl(available, used);
        }

        public string NodeIdentifier { get; }
        public string ProcessId { get; }
        public FileDescriptorChurnMetrics FileDescriptors { get; }
        public SocketDescriptorChurnMetrics SocketDescriptors { get; }

        
        class FileDescriptorChurnMetricsImpl :
            FileDescriptorChurnMetrics
        {
            public FileDescriptorChurnMetricsImpl(ulong available, ulong used)
            {
                Available = available;
                Used = used;
            }

            public ulong Available { get; }
            public ulong Used { get; }
            public decimal UsageRate { get; }
            public ulong OpenAttempts { get; }
            public decimal OpenAttemptRate { get; }
            public decimal AvgTimePerOpenAttempt { get; }
            public decimal AvgTimeRatePerOpenAttempt { get; }
        }
    }
}