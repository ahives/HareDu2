// Copyright 2013-2019 Albert L. Hives
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
    using Extensions;
    using Internal;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class ConfigurationProvider :
        IConfigurationProvider
    {
        public bool TryGet(string path, out HareDuConfig config)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new HyphenatedNamingConvention())
                    .Build();

                if (!File.Exists(path))
                {
                    config = ConfigCache.Default;
                    return true;
                }

                using (var reader = File.OpenText(path))
                {
                    var deserializedConfig = deserializer.Deserialize<HareDuConfigYaml>(reader);
                    
                    config = new HareDuConfigImpl(deserializedConfig);

                    return Validate(config);
                }
            }
            catch (Exception e)
            {
                config = ConfigCache.Default;
                return true;
            }
        }

        bool Validate(HareDuConfig config)
        {
            if (config.IsNull() ||
                config.Broker.Credentials.IsNull() ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Username) ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Password) ||
                string.IsNullOrWhiteSpace(config.Broker.BrokerUrl))
            {
                return false;
            }

            return true;
        }


        class HareDuConfigImpl :
            HareDuConfig
        {
            public HareDuConfigImpl(HareDuConfigYaml config)
            {
                Broker = new BrokerConfigImpl(config.Broker);
                // OverrideAnalyzerConfig = config.OverrideAnalyzerConfig;
                Analyzer = new DiagnosticAnalyzerConfigImpl(config.Analyzer);
            }

            public BrokerConfig Broker { get; }
            public bool OverrideAnalyzerConfig { get; }
            public DiagnosticAnalyzerConfig Analyzer { get; }

            
            class BrokerConfigImpl :
                BrokerConfig
            {
                public BrokerConfigImpl(RabbitMqBrokerConfigYaml config)
                {
                    BrokerUrl = config.BrokerUrl;
                    Timeout = config.Timeout;
                    Credentials = new BrokerCredentialsImpl(config.Username, config.Password);
                }

                public string BrokerUrl { get; }
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

            
            class DiagnosticAnalyzerConfigImpl :
                DiagnosticAnalyzerConfig
            {
                public DiagnosticAnalyzerConfigImpl(DiagnosticAnalyzerConfigYaml config)
                {
                    HighClosureRateWarningThreshold = config.HighClosureRateWarningThreshold;
                    HighCreationRateWarningThreshold = config.HighCreationRateWarningThreshold;
                    QueueHighFlowThreshold = config.QueueHighFlowThreshold;
                    QueueLowFlowThreshold = config.QueueLowFlowThreshold;
                    MessageRedeliveryCoefficient = config.MessageRedeliveryCoefficient;
                    SocketUsageCoefficient = config.SocketUsageCoefficient;
                    RuntimeProcessUsageCoefficient = config.RuntimeProcessUsageCoefficient;
                    FileDescriptorUsageWarningCoefficient = config.FileDescriptorUsageWarningCoefficient;
                    ConsumerUtilizationWarningCoefficient = config.ConsumerUtilizationWarningCoefficient;
                }

                public uint HighClosureRateWarningThreshold { get; }
                public uint HighCreationRateWarningThreshold { get; }
                public uint QueueHighFlowThreshold { get; }
                public uint QueueLowFlowThreshold { get; }
                public decimal MessageRedeliveryCoefficient { get; }
                public decimal SocketUsageCoefficient { get; }
                public decimal RuntimeProcessUsageCoefficient { get; }
                public decimal FileDescriptorUsageWarningCoefficient { get; }
                public decimal ConsumerUtilizationWarningCoefficient { get; }
            }
        }
    }
}