// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Core.Configuration
{
    using System;
    using System.IO;
    using Internal;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class YamlFileConfigProvider :
        IFileConfigProvider
    {
        readonly IConfigValidator _validator;
        readonly IDeserializer _deserializer;

        public YamlFileConfigProvider(IConfigValidator validator)
        {
            _validator = validator;
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();
        }

        public YamlFileConfigProvider()
        {
            _validator = new HareDuConfigValidator();
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();
        }

        public bool TryGet(string path, out HareDuConfig config)
        {
            try
            {
                if (!File.Exists(path))
                {
                    config = ConfigCache.Default;
                    return true;
                }

                using (var reader = File.OpenText(path))
                {
                    var deserializedConfig = _deserializer.Deserialize<HareDuConfigYaml>(reader);
                    
                    config = new HareDuConfigImpl(deserializedConfig);

                    return _validator.Validate(config);
                }
            }
            catch (Exception e)
            {
                config = ConfigCache.Default;
                return true;
            }
        }


        class HareDuConfigImpl :
            HareDuConfig
        {
            public HareDuConfigImpl(HareDuConfigYaml config)
            {
                Broker = new BrokerConfigImpl(config.Broker);
                Diagnostics = new DiagnosticsConfigImpl(config.Diagnostics);
            }

            public BrokerConfig Broker { get; }
            public bool OverrideAnalyzerConfig { get; }
            public DiagnosticsConfig Diagnostics { get; }

            
            class BrokerConfigImpl :
                BrokerConfig
            {
                public BrokerConfigImpl(RabbitMqBrokerConfigYaml config)
                {
                    Url = config.BrokerUrl;
                    Timeout = config.Timeout;
                    Credentials = new BrokerCredentialsImpl(config.Username, config.Password);
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

            
            class DiagnosticsConfigImpl :
                DiagnosticsConfig
            {
                public DiagnosticsConfigImpl(DiagnosticsConfigYaml config)
                {
                    Probes = new ProbesConfigImpl(config.Probes);
                }

                public ProbesConfig Probes { get; }

                
                class ProbesConfigImpl :
                    ProbesConfig
                {
                    public ProbesConfigImpl(DiagnosticProbeConfigYaml config)
                    {
                        HighClosureRateThreshold = config.HighClosureRateThreshold;
                        HighCreationRateThreshold = config.HighCreationRateThreshold;
                        QueueHighFlowThreshold = config.QueueHighFlowThreshold;
                        QueueLowFlowThreshold = config.QueueLowFlowThreshold;
                        MessageRedeliveryThresholdCoefficient = config.MessageRedeliveryCoefficient;
                        SocketUsageThresholdCoefficient = config.SocketUsageThresholdCoefficient;
                        RuntimeProcessUsageThresholdCoefficient = config.RuntimeProcessUsageThresholdCoefficient;
                        FileDescriptorUsageThresholdCoefficient = config.FileDescriptorUsageThresholdCoefficient;
                        ConsumerUtilizationThreshold = config.ConsumerUtilizationThreshold;
                    }

                    public uint HighClosureRateThreshold { get; }
                    public uint HighCreationRateThreshold { get; }
                    public uint QueueHighFlowThreshold { get; }
                    public uint QueueLowFlowThreshold { get; }
                    public decimal MessageRedeliveryThresholdCoefficient { get; }
                    public decimal SocketUsageThresholdCoefficient { get; }
                    public decimal RuntimeProcessUsageThresholdCoefficient { get; }
                    public decimal FileDescriptorUsageThresholdCoefficient { get; }
                    public decimal ConsumerUtilizationThreshold { get; }
                }
            }
        }
    }
}