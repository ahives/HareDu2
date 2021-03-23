namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class NodeMemoryUsageInfoImpl
    {
        [JsonPropertyName("memory")]
        public MemoryInfoImpl Memory { get; set; }
    }
}