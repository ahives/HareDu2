namespace HareDu.Snapshotting.Model
{
    public interface ContextSwitchingDetails
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}