namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface OperatorPolicyInfo
    {
        [JsonPropertyName("vhost")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string VirtualHost { get; }
        
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string Name { get; }
        
        [JsonPropertyName("pattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string Pattern { get; }
        
        [JsonPropertyName("apply-to")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string AppliedTo { get; }
        
        [JsonPropertyName("definition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        IDictionary<string, ulong> Definition { get; }
        
        [JsonPropertyName("priority")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        int Priority { get; }
    }
}