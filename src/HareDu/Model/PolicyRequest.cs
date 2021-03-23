namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class PolicyRequest
    {
        public PolicyRequest(string pattern, IDictionary<string, string> arguments, int priority, PolicyAppliedTo applyTo)
        {
            Pattern = pattern;
            Arguments = arguments;
            Priority = priority;
            ApplyTo = applyTo;
        }

        public PolicyRequest()
        {
        }

        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }

        [JsonPropertyName("definition")]
        public IDictionary<string, string> Arguments { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("apply-to")]
        public PolicyAppliedTo ApplyTo { get; set; }
    }
}