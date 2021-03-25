namespace HareDu.Snapshotting.Model
{
    public interface SocketDescriptorChurnMetrics
    {
        ulong Available { get; }

        ulong Used { get; }

        decimal UsageRate { get; }
    }
}