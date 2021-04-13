namespace HareDu.Core.Configuration
{
    public interface ProbesConfig
    {
        uint HighConnectionClosureRateThreshold { get; }
        
        uint HighConnectionCreationRateThreshold { get; }
        
        uint QueueHighFlowThreshold { get; }
        
        uint QueueLowFlowThreshold { get; }
        
        decimal MessageRedeliveryThresholdCoefficient { get; }

        decimal SocketUsageThresholdCoefficient { get; }
        
        decimal RuntimeProcessUsageThresholdCoefficient { get; }
        
        decimal FileDescriptorUsageThresholdCoefficient { get; }
        
        decimal ConsumerUtilizationThreshold { get; }
    }
}