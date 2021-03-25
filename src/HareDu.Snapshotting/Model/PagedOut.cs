namespace HareDu.Snapshotting.Model
{
    public interface PagedOut
    {
        ulong Total { get; }
        
        ulong Bytes { get; }
    }
}