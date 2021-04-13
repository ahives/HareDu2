namespace HareDu.Core.Configuration
{
    public interface DiagnosticProbesConfigurator
    {
        void SetHighConnectionClosureRateThreshold(uint value);
        
        void SetHighConnectionCreationRateThreshold(uint value);
        
        void SetQueueHighFlowThreshold(uint value);
        
        void SetQueueLowFlowThreshold(uint value);
        
        void SetMessageRedeliveryThresholdCoefficient(decimal value);
        
        void SetSocketUsageThresholdCoefficient(decimal value);
        
        void SetRuntimeProcessUsageThresholdCoefficient(decimal value);
        
        void SetFileDescriptorUsageThresholdCoefficient(decimal value);
        
        void SetConsumerUtilizationThreshold(decimal value);
    }
}