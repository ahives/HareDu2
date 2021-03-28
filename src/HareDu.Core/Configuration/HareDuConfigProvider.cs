namespace HareDu.Core.Configuration
{
    using System;
    using System.Threading;
    using Extensions;

    public class HareDuConfigProvider :
        IHareDuConfigProvider
    {
        public HareDuConfig Configure(Action<HareDuConfigurator> configurator)
        {
            if (configurator == null)
                return ConfigCache.Default;
            
            var impl = new HareDuConfiguratorImpl();
            configurator(impl);

            HareDuConfig config = impl.Settings.Value;

            return Validate(config) ? config : ConfigCache.Default;
        }

        bool Validate(HareDuConfig config) => Validate(config.Broker) && Validate(config.Diagnostics);

        static bool Validate(DiagnosticsConfig config) =>
            !config.IsNull()
            && !config.Probes.IsNull()
            && config.Probes.ConsumerUtilizationThreshold > 0
            && config.Probes.HighConnectionClosureRateThreshold > 0
            && config.Probes.HighConnectionCreationRateThreshold > 0
            && config.Probes.MessageRedeliveryThresholdCoefficient > 0
            && config.Probes.QueueHighFlowThreshold > 0
            && config.Probes.QueueLowFlowThreshold > 0
            && config.Probes.SocketUsageThresholdCoefficient > 0
            && config.Probes.FileDescriptorUsageThresholdCoefficient > 0
            && config.Probes.RuntimeProcessUsageThresholdCoefficient > 0;

        static bool Validate(BrokerConfig config)
            => !config.IsNull() &&
                !config.Credentials.IsNull() &&
                !string.IsNullOrWhiteSpace(config.Credentials.Username) &&
                !string.IsNullOrWhiteSpace(config.Credentials.Password) &&
                !string.IsNullOrWhiteSpace(config.Url);

        
        class HareDuConfiguratorImpl :
            HareDuConfigurator
        {
            DiagnosticsConfig _diagnosticsSettings;
            BrokerConfig _brokerConfig;

            public Lazy<HareDuConfig> Settings { get; }

            public HareDuConfiguratorImpl()
            {
                Settings =
                    new Lazy<HareDuConfig>(() => new HareDuConfigImpl(_brokerConfig, _diagnosticsSettings), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Diagnostics(Action<DiagnosticsConfigurator> configurator)
            {
                if (configurator == null)
                    _diagnosticsSettings = ConfigCache.Default.Diagnostics;

                var impl = new DiagnosticsConfiguratorImpl();
                configurator(impl);

                DiagnosticsConfig config = impl.Settings.Value;

                _diagnosticsSettings = Validate(config) ? config : ConfigCache.Default.Diagnostics;
            }

            public void Broker(Action<BrokerConfigurator> configurator)
            {
                if (configurator == null)
                    _brokerConfig = ConfigCache.Default.Broker;

                var impl = new BrokerConfiguratorImpl();
                configurator(impl);

                BrokerConfig config = impl.Settings.Value;

                _brokerConfig = Validate(config) ? config : ConfigCache.Default.Broker;
            }

            
            class HareDuConfigImpl :
                HareDuConfig
            {
                public HareDuConfigImpl(BrokerConfig broker, DiagnosticsConfig diagnostics)
                {
                    Broker = broker;
                    Diagnostics = diagnostics;
                }

                public BrokerConfig Broker { get; }
                public DiagnosticsConfig Diagnostics { get; }
            }


            class BrokerConfiguratorImpl :
                BrokerConfigurator
            {
                string _url;
                TimeSpan _timeout;
                string _username;
                string _password;

                public Lazy<BrokerConfig> Settings { get; }

                public BrokerConfiguratorImpl()
                {
                    Settings = new Lazy<BrokerConfig>(
                        () => new BrokerConfigImpl(_url, _timeout, _username, _password),
                        LazyThreadSafetyMode.PublicationOnly);
                }

                public void ConnectTo(string url) => _url = url;

                public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

                public void UsingCredentials(string username, string password)
                {
                    _username = username;
                    _password = password;
                }


                class BrokerConfigImpl :
                    BrokerConfig
                {
                    public BrokerConfigImpl(string brokerUrl, TimeSpan timeout, string username, string password)
                    {
                        Url = brokerUrl;
                        Timeout = timeout;

                        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                            Credentials = new BrokerCredentialsImpl(username, password);
                    }

                    public string Url { get; }
                    public TimeSpan Timeout { get; }
                    public BrokerCredentials Credentials { get; }


                    class BrokerCredentialsImpl :
                        BrokerCredentials
                    {
                        public BrokerCredentialsImpl(string username, string password)
                        {
                            Username = username;
                            Password = password;
                        }

                        public string Username { get; }
                        public string Password { get; }
                    }
                }
            }


            class DiagnosticProbesConfiguratorImpl :
                DiagnosticProbesConfigurator
            {
                uint _highClosureRateWarningThreshold;
                uint _highCreationRateWarningThreshold;
                uint _queueHighFlowThreshold;
                uint _queueLowFlowThreshold;
                decimal _messageRedeliveryCoefficient;
                decimal _socketUsageCoefficient;
                decimal _runtimeProcessUsageCoefficient;
                decimal _fileDescriptorUsageWarningCoefficient;
                decimal _consumerUtilizationWarningCoefficient;

                public Lazy<ProbesConfig> Settings { get; }

                public DiagnosticProbesConfiguratorImpl()
                {
                    Settings = new Lazy<ProbesConfig>(
                        () => new ProbesImpl(
                            _highClosureRateWarningThreshold,
                            _highCreationRateWarningThreshold,
                            _queueHighFlowThreshold,
                            _queueLowFlowThreshold,
                            _messageRedeliveryCoefficient,
                            _socketUsageCoefficient,
                            _runtimeProcessUsageCoefficient,
                            _fileDescriptorUsageWarningCoefficient,
                            _consumerUtilizationWarningCoefficient), LazyThreadSafetyMode.PublicationOnly);
                }

                public void SetHighConnectionClosureRateThreshold(uint value) => _highClosureRateWarningThreshold = value;

                public void SetHighConnectionCreationRateThreshold(uint value) => _highCreationRateWarningThreshold = value;

                public void SetQueueHighFlowThreshold(uint value) => _queueHighFlowThreshold = value;

                public void SetQueueLowFlowThreshold(uint value) => _queueLowFlowThreshold = value;

                public void SetMessageRedeliveryThresholdCoefficient(decimal value) =>
                    _messageRedeliveryCoefficient = value;

                public void SetSocketUsageThresholdCoefficient(decimal value) => _socketUsageCoefficient = value;

                public void SetRuntimeProcessUsageThresholdCoefficient(decimal value) =>
                    _runtimeProcessUsageCoefficient = value;

                public void SetFileDescriptorUsageThresholdCoefficient(decimal value) =>
                    _fileDescriptorUsageWarningCoefficient = value;

                public void SetConsumerUtilizationThreshold(decimal value) =>
                    _consumerUtilizationWarningCoefficient = value;


                class ProbesImpl :
                    ProbesConfig
                {
                    public ProbesImpl(uint highClosureRateThreshold,
                        uint highCreationRateThreshold,
                        uint queueHighFlowThreshold,
                        uint queueLowFlowThreshold,
                        decimal messageRedeliveryThresholdCoefficient,
                        decimal socketUsageThresholdCoefficient,
                        decimal runtimeProcessUsageThresholdCoefficient,
                        decimal fileDescriptorUsageThresholdCoefficient,
                        decimal consumerUtilizationThreshold)
                    {
                        HighConnectionClosureRateThreshold = highClosureRateThreshold;
                        HighConnectionCreationRateThreshold = highCreationRateThreshold;
                        QueueHighFlowThreshold = queueHighFlowThreshold;
                        QueueLowFlowThreshold = queueLowFlowThreshold;
                        MessageRedeliveryThresholdCoefficient = messageRedeliveryThresholdCoefficient;
                        SocketUsageThresholdCoefficient = socketUsageThresholdCoefficient;
                        RuntimeProcessUsageThresholdCoefficient = runtimeProcessUsageThresholdCoefficient;
                        FileDescriptorUsageThresholdCoefficient = fileDescriptorUsageThresholdCoefficient;
                        ConsumerUtilizationThreshold = consumerUtilizationThreshold;
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

            
            class DiagnosticsConfiguratorImpl :
                DiagnosticsConfigurator
            {
                ProbesConfig _probes;
                
                public Lazy<DiagnosticsConfig> Settings { get; }

                public DiagnosticsConfiguratorImpl()
                {
                    Settings = new Lazy<DiagnosticsConfig>(
                        () => new DiagnosticsConfigImpl(_probes), LazyThreadSafetyMode.PublicationOnly);
                }

                public void Probes(Action<DiagnosticProbesConfigurator> configurator)
                {
                    var impl = new DiagnosticProbesConfiguratorImpl();
                    configurator(impl);

                    _probes = impl.Settings.Value;
                }


                class DiagnosticsConfigImpl :
                    DiagnosticsConfig
                {
                    public DiagnosticsConfigImpl(ProbesConfig probes)
                    {
                        Probes = probes;
                    }

                    public ProbesConfig Probes { get; }
                }
            }
        }
    }
}