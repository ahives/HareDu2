namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class PolicyInfoImpl
    {
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }
        
        [JsonPropertyName("apply-to")]
        public string AppliedTo { get; set; }

        [JsonPropertyName("definition")]
        public IDictionary<string, string> Definition { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }
    }
}