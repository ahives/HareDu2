namespace HareDu.Core.Configuration.Internal
{
    public class InternalProbesConfig
    {
        public uint HighConnectionClosureRateThreshold { get; set; }
        public uint HighConnectionCreationRateThreshold { get; set; }
        public uint QueueHighFlowThreshold { get; set; }
        public uint QueueLowFlowThreshold { get; set; }
        public decimal MessageRedeliveryThresholdCoefficient { get; set; }
        public decimal SocketUsageThresholdCoefficient { get; set; }
        public decimal RuntimeProcessUsageThresholdCoefficient { get; set; }
        public decimal FileDescriptorUsageThresholdCoefficient { get; set; }
        public decimal ConsumerUtilizationThreshold { get; set; }
    }
}