namespace HareDu.Snapshotting.Model
{
    public interface CollectedGarbage
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}