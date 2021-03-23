namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class TopicPermissionsRequest
    {
        public TopicPermissionsRequest(string exchange, string write, string read)
        {
            Exchange = exchange;
            Write = write;
            Read = read;
        }

        public TopicPermissionsRequest()
        {
        }

        [JsonPropertyName("exchange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Exchange { get; set; }
        
        [JsonPropertyName("write")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Read { get; set; }
    }
}