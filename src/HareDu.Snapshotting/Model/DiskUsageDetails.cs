namespace HareDu.Snapshotting.Model
{
    public interface DiskUsageDetails
    {
        ulong Total { get; }
        
        decimal Rate { get; }
        
        Bytes Bytes { get; }
        
        DiskOperationWallTime WallTime { get; }
    }
}