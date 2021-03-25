namespace HareDu.Snapshotting.Model
{
    public interface MessageStoreDetails
    {
        ulong Total { get; }
        
        decimal Rate { get; }
    }
}