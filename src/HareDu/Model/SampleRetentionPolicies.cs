namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface SampleRetentionPolicies
    {
        [JsonPropertyName("global")]
        IList<ulong> Global { get; }
        
        [JsonPropertyName("basic")]
        IList<ulong> Basic { get; }
        
        [JsonPropertyName("detailed")]
        IList<ulong> Detailed { get; }
    }
}