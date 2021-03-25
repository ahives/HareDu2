namespace HareDu.Snapshotting.Model
{
    public interface IndexUsageDetails
    {
        ulong Total { get; }

        decimal Rate { get; }
    }
}