namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class QueueDefinition
    {
        public QueueDefinition(string node, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            Node = node;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public QueueDefinition()
        {
        }

        [JsonPropertyName("node")]
        public string Node { get; set; }
        
        [JsonPropertyName("durable")]
        public bool Durable { get; set; }
        
        [JsonPropertyName("auto_delete")]
        public bool AutoDelete { get; set; }
                
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
    }
}