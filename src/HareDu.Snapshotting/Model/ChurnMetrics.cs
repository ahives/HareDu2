namespace HareDu.Snapshotting.Model
{
    public interface ChurnMetrics
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}