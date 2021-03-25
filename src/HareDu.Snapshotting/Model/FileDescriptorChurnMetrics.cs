namespace HareDu.Snapshotting.Model
{
    public interface FileDescriptorChurnMetrics
    {
        ulong Available { get; }

        ulong Used { get; }

        decimal UsageRate { get; }

        ulong OpenAttempts { get; }

        decimal OpenAttemptRate { get; }

        decimal AvgTimePerOpenAttempt { get; }

        decimal AvgTimeRatePerOpenAttempt { get; }
    }
}