namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class VirtualHostRequest
    {
        public VirtualHostRequest(bool tracing, string description, string tags)
        {
            Tracing = tracing;
            Description = description;
            Tags = tags;
        }

        public VirtualHostRequest()
        {
        }

        [JsonPropertyName("tracing")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Tracing { get; set; }
        
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Description { get; set; }
        
        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Tags { get; set; }
    }
}