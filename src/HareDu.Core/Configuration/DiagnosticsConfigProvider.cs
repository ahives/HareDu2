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
    using System.Threading;

    public class DiagnosticsConfigProvider :
        IDiagnosticsConfigProvider
    {
        public DiagnosticsConfig Configure(Action<DiagnosticProbesConfigurator> configurator)
        {
            if (configurator == null)
                return ConfigCache.Default.Diagnostics;
            
            var impl = new DiagnosticProbesConfiguratorImpl();
            configurator(impl);

            DiagnosticsConfig config = impl.Settings.Value;

            return Validate(config) ? config : ConfigCache.Default.Diagnostics;
        }

        bool Validate(DiagnosticsConfig config)
        {
            return true;
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
            
            public Lazy<DiagnosticsConfig> Settings { get; }

            public DiagnosticProbesConfiguratorImpl()
            {
                Settings = new Lazy<DiagnosticsConfig>(
                    () => new DiagnosticsConfigImpl(
                        new ProbesImpl(
                            _highClosureRateWarningThreshold,
                            _highCreationRateWarningThreshold,
                            _queueHighFlowThreshold,
                            _queueLowFlowThreshold,
                            _messageRedeliveryCoefficient,
                            _socketUsageCoefficient,
                            _runtimeProcessUsageCoefficient,
                            _fileDescriptorUsageWarningCoefficient,
                            _consumerUtilizationWarningCoefficient)), LazyThreadSafetyMode.PublicationOnly);
            }

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
                    HighClosureRateThreshold = highClosureRateThreshold;
                    HighCreationRateThreshold = highCreationRateThreshold;
                    QueueHighFlowThreshold = queueHighFlowThreshold;
                    QueueLowFlowThreshold = queueLowFlowThreshold;
                    MessageRedeliveryThresholdCoefficient = messageRedeliveryThresholdCoefficient;
                    SocketUsageThresholdCoefficient = socketUsageThresholdCoefficient;
                    RuntimeProcessUsageThresholdCoefficient = runtimeProcessUsageThresholdCoefficient;
                    FileDescriptorUsageThresholdCoefficient = fileDescriptorUsageThresholdCoefficient;
                    ConsumerUtilizationThreshold = consumerUtilizationThreshold;
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


            class DiagnosticsConfigImpl :
                DiagnosticsConfig
            {
                public DiagnosticsConfigImpl(ProbesConfig probes)
                {
                    Probes = probes;
                }

                public ProbesConfig Probes { get; }
            }

            public void SetHighClosureRateThreshold(uint value) => _highClosureRateWarningThreshold = value;

            public void SetHighCreationRateThreshold(uint value) => _highCreationRateWarningThreshold = value;

            public void SetQueueHighFlowThreshold(uint value) => _queueHighFlowThreshold = value;

            public void SetQueueLowFlowThreshold(uint value) => _queueLowFlowThreshold = value;

            public void SetMessageRedeliveryThresholdCoefficient(decimal value) => _messageRedeliveryCoefficient = value;

            public void SetSocketUsageThresholdCoefficient(decimal value) => _socketUsageCoefficient = value;

            public void SetRuntimeProcessUsageThresholdCoefficient(decimal value) => _runtimeProcessUsageCoefficient = value;

            public void SetFileDescriptorUsageThresholdCoefficient(decimal value) => _fileDescriptorUsageWarningCoefficient = value;

            public void SetConsumerUtilizationThreshold(decimal value) => _consumerUtilizationWarningCoefficient = value;
        }
    }
}