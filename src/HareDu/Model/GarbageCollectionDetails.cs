namespace HareDu.Model
{
    public interface GarbageCollectionDetails
    {
        string MinorGarbageCollection { get; }

        long FullSweepAfter { get; }

        long MinimumHeapSize { get; }

        long MinimumBinaryVirtualHeapSize { get; }

        long MaximumHeapSize { get; }
    }
}