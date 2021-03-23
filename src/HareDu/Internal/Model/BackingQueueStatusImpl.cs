namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class BackingQueueStatusImpl
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("q1")]
        public long Q1 { get; set; }
        
        [JsonPropertyName("q2")]
        public long Q2 { get; set; }
        
        [JsonPropertyName("delta")]
        public IList<object> Delta { get; set; }
        
        [JsonPropertyName("q3")]
        public long Q3 { get; set; }
        
        [JsonPropertyName("q4")]
        public long Q4 { get; set; }
        
        [JsonPropertyName("len")]
        public long Length { get; set; }
        
        [JsonPropertyName("target_ram_count")]
        public string TargetTotalMessagesInRAM { get; set; }
        
        [JsonPropertyName("next_seq_id")]
        public long NextSequenceId { get; set; }
        
        [JsonPropertyName("avg_ingress_rate")]
        public decimal AvgIngressRate { get; set; }
        
        [JsonPropertyName("avg_egress_rate")]
        public decimal AvgEgressRate { get; set; }
        
        [JsonPropertyName("avg_ack_ingress_rate")]
        public decimal AvgAcknowledgementIngressRate { get; set; }
        
        [JsonPropertyName("avg_ack_egress_rate")]
        public decimal AvgAcknowledgementEgressRate { get; set; }
    }
}