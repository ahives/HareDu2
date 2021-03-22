namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class GarbageCollectionDetailsImpl
    {
        [JsonPropertyName("minor_gcs")]
        public string MinorGarbageCollection { get; set; }

        [JsonPropertyName("fullsweep_after")]
        public long FullSweepAfter { get; set; }

        [JsonPropertyName("min_heap_size")]
        public long MinimumHeapSize { get; set; }

        [JsonPropertyName("min_bin_vheap_size")]
        public long MinimumBinaryVirtualHeapSize { get; set; }

        [JsonPropertyName("max_heap_size")]
        public long MaximumHeapSize { get; set; }
    }
}