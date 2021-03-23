namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class QueuePeekDefinition
    {
        public QueuePeekDefinition(uint take, string encoding, ulong truncateMessageThreshold, string requeueMode)
        {
            Take = take;
            Encoding = encoding;
            TruncateMessageThreshold = truncateMessageThreshold;
            RequeueMode = requeueMode;
        }

        public QueuePeekDefinition()
        {
        }

        [JsonPropertyName("count")]
        public uint Take { get; set; }
        
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }
        
        [JsonPropertyName("truncate")]
        public ulong TruncateMessageThreshold { get; set; }
        
        [JsonPropertyName("ackmode")]
        public string RequeueMode { get; set; }
    }
}