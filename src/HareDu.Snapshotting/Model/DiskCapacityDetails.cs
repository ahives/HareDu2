namespace HareDu.Snapshotting.Model
{
    public interface DiskCapacityDetails
    {
        ulong Available { get; }
        
        decimal Rate { get; }
    }
}