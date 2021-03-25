namespace HareDu.Snapshotting.Model
{
    public interface TransactionDetails
    {
        PersistenceDetails RAM { get; }
        
        PersistenceDetails Disk { get; }
    }
}