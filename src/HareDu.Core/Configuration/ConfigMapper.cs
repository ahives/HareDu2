namespace HareDu.Core.Configuration
{
    using System;
    using Internal;

    public static class ConfigMapper
    {
        public static HareDuConfig Map(InternalHareDuConfig config) => new HareDuConfigImpl(config);


        class HareDuConfigImpl :
            HareDuConfig
        {
            public HareDuConfigImpl(InternalHareDuConfig config)
            {
                Broker = new BrokerConfigImpl(config.Broker);
                Diagnostics = new DiagnosticsConfigImpl(config.Diagnostics);
            }
        
            public BrokerConfig Broker { get; }
            public DiagnosticsConfig Diagnostics { get; }
        
            
            class DiagnosticsConfigImpl :
                DiagnosticsConfig
            {
                public DiagnosticsConfigImpl(InternalDiagnosticsConfig config)
                {
                    Probes = new ProbesConfigImpl(config.Probes);
                }
        
                public ProbesConfig Probes { get; }
        
                
                class ProbesConfigImpl :
                    ProbesConfig
                {
                    public ProbesConfigImpl(InternalProbesConfig config)
                    {
                        HighConnectionClosureRateThreshold = config.HighConnectionClosureRateThreshold;
                        HighConnectionCreationRateThreshold = config.HighConnectionCreationRateThreshold;
                        QueueHighFlowThreshold = config.QueueHighFlowThreshold;
                        QueueLowFlowThreshold = config.QueueLowFlowThreshold;
                        MessageRedeliveryThresholdCoefficient = config.MessageRedeliveryThresholdCoefficient;
                        SocketUsageThresholdCoefficient = config.SocketUsageThresholdCoefficient;
                        RuntimeProcessUsageThresholdCoefficient = config.RuntimeProcessUsageThresholdCoefficient;
                        FileDescriptorUsageThresholdCoefficient = config.FileDescriptorUsageThresholdCoefficient;
                        ConsumerUtilizationThreshold = config.ConsumerUtilizationThreshold;
                    }
        
                    public uint HighConnectionClosureRateThreshold { get; }
                    public uint HighConnectionCreationRateThreshold { get; }
                    public uint QueueHighFlowThreshold { get; }
                    public uint QueueLowFlowThreshold { get; }
                    public decimal MessageRedeliveryThresholdCoefficient { get; }
                    public decimal SocketUsageThresholdCoefficient { get; }
                    public decimal RuntimeProcessUsageThresholdCoefficient { get; }
                    public decimal FileDescriptorUsageThresholdCoefficient { get; }
                    public decimal ConsumerUtilizationThreshold { get; }
                }
            }
        
            
            class BrokerConfigImpl :
                BrokerConfig
            {
                public BrokerConfigImpl(InternalBrokerConfig config)
                {
                    Url = config.Url;
                    Timeout = config.Timeout;
                    Credentials = new BrokerCredentialsImpl(config.Credentials);
                }
        
                
                class BrokerCredentialsImpl :
                    BrokerCredentials
                {
                    public BrokerCredentialsImpl(InternalBrokerCredentials config)
                    {
                        Username = config.Username;
                        Password = config.Password;
                    }
        
                    public string Username { get; }
                    public string Password { get; }
                }
        
                public string Url { get; }
                public TimeSpan Timeout { get; }
                public BrokerCredentials Credentials { get; }
            }
        }
    }
}