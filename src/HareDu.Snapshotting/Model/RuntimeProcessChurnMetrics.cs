namespace HareDu.Snapshotting.Model
{
    public interface RuntimeProcessChurnMetrics
    {
        ulong Limit { get; }
        
        ulong Used { get; }

        decimal UsageRate { get; }
    }
}