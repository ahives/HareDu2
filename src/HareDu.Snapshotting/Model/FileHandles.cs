namespace HareDu.Snapshotting.Model
{
    public interface FileHandles
    {
        ulong Recycled { get; }
        
        decimal Rate { get; }
    }
}