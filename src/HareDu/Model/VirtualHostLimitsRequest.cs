namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class VirtualHostLimitsRequest
    {
        public VirtualHostLimitsRequest(ulong maxConnectionLimit, ulong maxQueueLimit)
        {
            MaxConnectionLimit = maxConnectionLimit;
            MaxQueueLimit = maxQueueLimit;
        }

        public VirtualHostLimitsRequest()
        {
        }

        [JsonPropertyName("max-connections")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ulong MaxConnectionLimit { get; set; }
        
        [JsonPropertyName("max-queues")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ulong MaxQueueLimit { get; set; }
    }
}