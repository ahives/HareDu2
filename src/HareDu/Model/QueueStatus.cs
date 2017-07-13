namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface QueueStatus
    {
        [JsonProperty("mode")]
        string Mode { get; }
        
        [JsonProperty("q1")]
        long Q1 { get; }
        
        [JsonProperty("q2")]
        long Q2 { get; }
        
        [JsonProperty("delta")]
        IEnumerable<object> Delta { get; }
        
        [JsonProperty("q3")]
        long Q3 { get; }
        
        [JsonProperty("q4")]
        long Q4 { get; }
        
        [JsonProperty("len")]
        long Length { get; }
        
        [JsonProperty("target_ram_count")]
        string TotalTargetRam { get; }
        
        [JsonProperty("next_seq_id")]
        long NextSequenceId { get; }
        
        [JsonProperty("avg_ingress_rate")]
        decimal AvgIngressRate { get; }
        
        [JsonProperty("avg_egress_rate")]
        decimal AvgEgressRate { get; }
        
        [JsonProperty("avg_ack_ingress_rate")]
        decimal AvgAcknowledgementIngressRate { get; }
        
        [JsonProperty("avg_ack_egress_rate")]
        decimal AvgAcknowledgementEgressRate { get; }
    }
}