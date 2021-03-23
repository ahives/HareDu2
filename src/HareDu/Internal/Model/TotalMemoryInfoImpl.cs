namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class TotalMemoryInfoImpl
    {
        [JsonPropertyName("erlang")]
        public long Erlang { get; set; }
        
        [JsonPropertyName("rss")]
        public long Strategy { get; set; }
        
        [JsonPropertyName("allocated")]
        public long Allocated { get; set; }
    }
}