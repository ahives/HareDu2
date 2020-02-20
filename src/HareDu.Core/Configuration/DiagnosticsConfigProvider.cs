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
        public DiagnosticsConfig Configure(Action<DiagnosticsConfigurator> configurator)
        {
            if (configurator == null)
                return ConfigCache.Default.Diagnostics;
            
            var impl = new DiagnosticsConfiguratorImpl();
            configurator(impl);

            DiagnosticsConfig config = impl.Settings.Value;

            return Validate(config) ? config : ConfigCache.Default.Diagnostics;
        }

        bool Validate(DiagnosticsConfig config)
        {
            return true;
        }


        class DiagnosticsConfiguratorImpl :
            DiagnosticsConfigurator
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

            public DiagnosticsConfiguratorImpl()
            {
                Settings = new Lazy<DiagnosticsConfig>(
                    () => new DiagnosticsConfigImpl(
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

            
            class DiagnosticsConfigImpl :
                DiagnosticsConfig
            {
                public DiagnosticsConfigImpl(uint highClosureRateWarningThreshold,
                    uint highCreationRateWarningThreshold,
                    uint queueHighFlowThreshold,
                    uint queueLowFlowThreshold,
                    decimal messageRedeliveryCoefficient,
                    decimal socketUsageCoefficient,
                    decimal runtimeProcessUsageCoefficient,
                    decimal fileDescriptorUsageWarningCoefficient,
                    decimal consumerUtilizationWarningCoefficient)
                {
                    HighClosureRateWarningThreshold = highClosureRateWarningThreshold;
                    HighCreationRateWarningThreshold = highCreationRateWarningThreshold;
                    QueueHighFlowThreshold = queueHighFlowThreshold;
                    QueueLowFlowThreshold = queueLowFlowThreshold;
                    MessageRedeliveryCoefficient = messageRedeliveryCoefficient;
                    SocketUsageCoefficient = socketUsageCoefficient;
                    RuntimeProcessUsageCoefficient = runtimeProcessUsageCoefficient;
                    FileDescriptorUsageWarningCoefficient = fileDescriptorUsageWarningCoefficient;
                    ConsumerUtilizationWarningCoefficient = consumerUtilizationWarningCoefficient;
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

            public void SetHighClosureRateWarningThreshold(uint value) => _highClosureRateWarningThreshold = value;

            public void SetHighCreationRateWarningThreshold(uint value) => _highCreationRateWarningThreshold = value;

            public void SetQueueHighFlowThreshold(uint value) => _queueHighFlowThreshold = value;

            public void SetQueueLowFlowThreshold(uint value) => _queueLowFlowThreshold = value;

            public void SetMessageRedeliveryCoefficient(decimal value) => _messageRedeliveryCoefficient = value;

            public void SetSocketUsageCoefficient(decimal value) => _socketUsageCoefficient = value;

            public void SetRuntimeProcessUsageCoefficient(decimal value) => _runtimeProcessUsageCoefficient = value;

            public void SetFileDescriptorUsageWarningCoefficient(decimal value) => _fileDescriptorUsageWarningCoefficient = value;

            public void SetConsumerUtilizationWarningCoefficient(decimal value) => _consumerUtilizationWarningCoefficient = value;
        }
    }
}