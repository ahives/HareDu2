namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;
    using HareDu.Model;

    class NodeHealthInfoImpl
    {
        [JsonPropertyName("status")]
        public NodeStatus Status { get; set; }

        [JsonPropertyName("reason")]
        public long Reason { get; set; }
    }
}