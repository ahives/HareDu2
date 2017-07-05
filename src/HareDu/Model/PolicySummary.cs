namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface PolicySummary
    {
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("pattern")]
        string Pattern { get; }
        
        [JsonProperty("apply-to")]
        string ApplyTo { get; }
        
        [JsonProperty("definition")]
        PolicyDefinition Definition { get; }
        
        [JsonProperty("priority")]
        long Priority { get; }
    }
}