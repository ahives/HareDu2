namespace HareDu.Snapshotting.Model
{
    public interface RuntimeDatabase
    {
        TransactionDetails Transactions { get; }
        
        IndexDetails Index { get; }
        
        StorageDetails Storage { get; }
    }
}