namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface BackingQueueStatus
    {
        [JsonPropertyName("mode")]
        string Mode { get; }
        
        [JsonPropertyName("q1")]
        long Q1 { get; }
        
        [JsonPropertyName("q2")]
        long Q2 { get; }
        
        [JsonPropertyName("delta")]
        IList<object> Delta { get; }
        
        [JsonPropertyName("q3")]
        long Q3 { get; }
        
        [JsonPropertyName("q4")]
        long Q4 { get; }
        
        [JsonPropertyName("len")]
        long Length { get; }
        
        [JsonPropertyName("target_ram_count")]
        string TargetTotalMessagesInRAM { get; }
        
        [JsonPropertyName("next_seq_id")]
        long NextSequenceId { get; }
        
        [JsonPropertyName("avg_ingress_rate")]
        decimal AvgIngressRate { get; }
        
        [JsonPropertyName("avg_egress_rate")]
        decimal AvgEgressRate { get; }
        
        [JsonPropertyName("avg_ack_ingress_rate")]
        decimal AvgAcknowledgementIngressRate { get; }
        
        [JsonPropertyName("avg_ack_egress_rate")]
        decimal AvgAcknowledgementEgressRate { get; }
    }
}