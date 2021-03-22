namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalGarbageCollectionDetailsImpl :
        GarbageCollectionDetails
    {
        public InternalGarbageCollectionDetailsImpl(GarbageCollectionDetailsImpl item)
        {
            MinorGarbageCollection = item.MinorGarbageCollection;
            FullSweepAfter = item.FullSweepAfter;
            MinimumHeapSize = item.MinimumHeapSize;
            MinimumBinaryVirtualHeapSize = item.MinimumBinaryVirtualHeapSize;
            MaximumHeapSize = item.MaximumHeapSize;
        }

        public string MinorGarbageCollection { get; }
        public long FullSweepAfter { get; }
        public long MinimumHeapSize { get; }
        public long MinimumBinaryVirtualHeapSize { get; }
        public long MaximumHeapSize { get; }
    }
}