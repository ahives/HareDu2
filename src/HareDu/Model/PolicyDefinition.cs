namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface PolicyDefinition
    {
        [JsonPropertyName("pattern")]
        string Pattern { get; }

        [JsonPropertyName("definition")]
        IDictionary<string, string> Arguments { get; }

        [JsonPropertyName("priority")]
        int Priority { get; }

        [JsonPropertyName("apply-to")]
        string ApplyTo { get; }
    }
}