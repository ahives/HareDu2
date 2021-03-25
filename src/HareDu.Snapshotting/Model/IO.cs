namespace HareDu.Snapshotting.Model
{
    public interface IO
    {
        DiskUsageDetails Reads { get; }
        
        DiskUsageDetails Writes { get; }
        
        DiskUsageDetails Seeks { get; }

        FileHandles FileHandles { get; }
    }
}