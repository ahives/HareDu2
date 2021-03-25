namespace HareDu.Snapshotting.Model
{
    public interface QueueDepth
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}