namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class SampleRetentionPoliciesImpl
    {
        [JsonPropertyName("global")]
        public IList<ulong> Global { get; set; }
        
        [JsonPropertyName("basic")]
        public IList<ulong> Basic { get; set; }
        
        [JsonPropertyName("detailed")]
        public IList<ulong> Detailed { get; set; }
    }
}