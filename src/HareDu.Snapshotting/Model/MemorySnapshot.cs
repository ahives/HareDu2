namespace HareDu.Snapshotting.Model
{
    public interface MemorySnapshot :
        Snapshot
    {
        string NodeIdentifier { get; }
        
        ulong Used { get; }
        
        decimal UsageRate { get; }

        ulong Limit { get; }

        bool AlarmInEffect { get; }
    }
}