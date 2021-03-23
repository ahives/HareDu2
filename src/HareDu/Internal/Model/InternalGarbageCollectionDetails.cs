namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalGarbageCollectionDetails :
        GarbageCollectionDetails
    {
        public InternalGarbageCollectionDetails(GarbageCollectionDetailsImpl obj)
        {
            MinorGarbageCollection = obj.MinorGarbageCollection;
            FullSweepAfter = obj.FullSweepAfter;
            MinimumHeapSize = obj.MinimumHeapSize;
            MinimumBinaryVirtualHeapSize = obj.MinimumBinaryVirtualHeapSize;
            MaximumHeapSize = obj.MaximumHeapSize;
        }

        public string MinorGarbageCollection { get; }
        public long FullSweepAfter { get; }
        public long MinimumHeapSize { get; }
        public long MinimumBinaryVirtualHeapSize { get; }
        public long MaximumHeapSize { get; }
    }
}