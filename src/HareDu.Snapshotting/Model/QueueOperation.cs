namespace HareDu.Snapshotting.Model
{
    public interface QueueOperation
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}