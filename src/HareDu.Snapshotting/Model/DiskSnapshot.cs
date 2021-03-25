namespace HareDu.Snapshotting.Model
{
    public interface DiskSnapshot :
        Snapshot
    {
        string NodeIdentifier { get; }
        
        DiskCapacityDetails Capacity { get; }

        ulong Limit { get; }

        bool AlarmInEffect { get; }
        
        IO IO { get; }
    }
}