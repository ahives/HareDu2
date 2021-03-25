namespace HareDu.Snapshotting.Model
{
    public interface StorageDetails
    {
        MessageStoreDetails Reads { get; }
        
        MessageStoreDetails Writes { get; }
    }
}