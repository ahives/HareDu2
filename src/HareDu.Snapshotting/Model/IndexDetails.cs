namespace HareDu.Snapshotting.Model
{
    public interface IndexDetails
    {
        IndexUsageDetails Reads { get; }
        
        IndexUsageDetails Writes { get; }
        
        JournalDetails Journal { get; }
    }
}