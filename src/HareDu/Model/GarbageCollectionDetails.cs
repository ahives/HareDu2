namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface GarbageCollectionDetails
    {
        [JsonProperty("minor_gcs")]
        string MinorGarbageCollection { get; }

        [JsonProperty("fullsweep_after")]
        long FullSweepAfter { get; }

        [JsonProperty("min_heap_size")]
        long MinimumHeapSize { get; }

        [JsonProperty("min_bin_vheap_size")]
        long MinimumBinaryVirtualHeapSize { get; }

        [JsonProperty("max_heap_size")]
        long MaximumHeapSize { get; }
    }
}