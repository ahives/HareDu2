namespace HareDu.Snapshotting.Model
{
    public interface Packets
    {
        ulong Total { get; }
        
        ulong Bytes { get; }
        
        decimal Rate { get; }
    }
}