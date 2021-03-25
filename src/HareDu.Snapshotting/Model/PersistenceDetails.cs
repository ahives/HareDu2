namespace HareDu.Snapshotting.Model
{
    public interface PersistenceDetails
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}