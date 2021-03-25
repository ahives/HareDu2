namespace HareDu.Snapshotting.Model
{
    public interface Bytes
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}