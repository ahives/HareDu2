namespace HareDu.Core.Configuration.Internal
{
    using YamlDotNet.Serialization;

    class DiagnosticProbeConfigYaml
    {
        [YamlMember(Alias = "high-connection-closure-rate-threshold")]
        public uint HighConnectionClosureRateThreshold { get; set; }
        
        [YamlMember(Alias = "high-connection-creation-rate-threshold")]
        public uint HighConnectionCreationRateThreshold { get; set; }
        
        [YamlMember(Alias = "queue-high-flow-threshold")]
        public uint QueueHighFlowThreshold { get; set; }
        
        [YamlMember(Alias = "queue-low-flow-threshold")]
        public uint QueueLowFlowThreshold { get; set; }
        
        [YamlMember(Alias = "message-redelivery-threshold-coefficient")]
        public decimal MessageRedeliveryCoefficient { get; set; }

        [YamlMember(Alias = "socket-usage-threshold-coefficient")]
        public decimal SocketUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "runtime-process-usage-threshold-coefficient")]
        public decimal RuntimeProcessUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "file-descriptor-usage-threshold-coefficient")]
        public decimal FileDescriptorUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "consumer-utilization-threshold")]
        public decimal ConsumerUtilizationThreshold { get; set; }
    }
}