namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface GarbageCollectionDetails
    {
        [JsonPropertyName("minor_gcs")]
        string MinorGarbageCollection { get; }

        [JsonPropertyName("fullsweep_after")]
        long FullSweepAfter { get; }

        [JsonPropertyName("min_heap_size")]
        long MinimumHeapSize { get; }

        [JsonPropertyName("min_bin_vheap_size")]
        long MinimumBinaryVirtualHeapSize { get; }

        [JsonPropertyName("max_heap_size")]
        long MaximumHeapSize { get; }
    }
}