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
namespace HareDu.Core.Configuration.Internal
{
    using System;

    class DefaultHareDuConfig :
        HareDuConfig
    {
        public DefaultHareDuConfig()
        {
            Diagnostics = new DiagnosticsConfigImpl();
            Broker = new BrokerConfigImpl();
        }

        public BrokerConfig Broker { get; }
        public DiagnosticsConfig Diagnostics { get; }

        
        class BrokerConfigImpl :
            BrokerConfig
        {
            public BrokerConfigImpl()
            {
                Url = "http://localhost:15672";
                Credentials = new BrokerCredentialsImpl("guest", "guest");
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
            public DiagnosticsConfigImpl()
            {
                Probes = new ProbesImpl();
            }

            public ProbesConfig Probes { get; }

            
            class ProbesImpl :
                ProbesConfig
            {
                public ProbesImpl()
                {
                    SocketUsageThresholdCoefficient = 0.50M;
                    MessageRedeliveryThresholdCoefficient = 1M;
                    HighConnectionClosureRateThreshold = 100;
                    HighConnectionCreationRateThreshold = 100;
                    RuntimeProcessUsageThresholdCoefficient = 0.7M;
                    FileDescriptorUsageThresholdCoefficient = 0.7M;
                    ConsumerUtilizationThreshold = 0.50M;
                    QueueLowFlowThreshold = 20;
                    QueueHighFlowThreshold = 100;
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
    }
}