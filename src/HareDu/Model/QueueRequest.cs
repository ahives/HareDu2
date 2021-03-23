namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class QueueRequest
    {
        public QueueRequest(string node, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            Node = node;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public QueueRequest()
        {
        }

        [JsonPropertyName("node")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Node { get; set; }
        
        [JsonPropertyName("durable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Durable { get; set; }
        
        [JsonPropertyName("auto_delete")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AutoDelete { get; set; }
                
        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IDictionary<string, object> Arguments { get; set; }
    }
}