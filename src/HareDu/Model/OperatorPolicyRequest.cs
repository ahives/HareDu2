namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class OperatorPolicyRequest
    {
        public OperatorPolicyRequest(string pattern, IDictionary<string, ulong> arguments, int priority, OperatorPolicyAppliedTo applyTo)
        {
            Pattern = pattern;
            Arguments = arguments;
            Priority = priority;
            ApplyTo = applyTo;
        }

        public OperatorPolicyRequest()
        {
        }

        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }

        [JsonPropertyName("definition")]
        public IDictionary<string, ulong> Arguments { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("apply-to")]
        public OperatorPolicyAppliedTo ApplyTo { get; set; }
    }
}