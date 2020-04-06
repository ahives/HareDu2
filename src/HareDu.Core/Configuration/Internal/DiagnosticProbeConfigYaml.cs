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
    using YamlDotNet.Serialization;

    class DiagnosticProbeConfigYaml
    {
        [YamlMember(Alias = "high-connection-closure-rate-threshold")]
        public uint HighConnectionClosureRateThreshold { get; set; }
        
        [YamlMember(Alias = "high-connection-creation-rate-threshold")]
        public uint HighConnectionCreationRateThreshold { get; set; }
        
        [YamlMember(Alias = "queue-high-flow-threshold")]
        public uint QueueHighFlowThreshold { get; set; }
        
        [YamlMember(Alias = "queue-low-flow-threshold")]
        public uint QueueLowFlowThreshold { get; set; }
        
        [YamlMember(Alias = "message-redelivery-threshold-coefficient")]
        public decimal MessageRedeliveryCoefficient { get; set; }

        [YamlMember(Alias = "socket-usage-threshold-coefficient")]
        public decimal SocketUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "runtime-process-usage-threshold-coefficient")]
        public decimal RuntimeProcessUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "file-descriptor-usage-threshold-coefficient")]
        public decimal FileDescriptorUsageThresholdCoefficient { get; set; }
        
        [YamlMember(Alias = "consumer-utilization-threshold")]
        public decimal ConsumerUtilizationThreshold { get; set; }
    }
}