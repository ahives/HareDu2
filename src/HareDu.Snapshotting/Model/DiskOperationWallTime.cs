namespace HareDu.Snapshotting.Model
{
    public interface DiskOperationWallTime
    {
        decimal Average { get; }
        
        decimal Rate { get; }
    }
}