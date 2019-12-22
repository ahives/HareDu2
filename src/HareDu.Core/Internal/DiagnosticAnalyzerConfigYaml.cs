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
namespace HareDu.Core.Internal
{
    using YamlDotNet.Serialization;

    class DiagnosticAnalyzerConfigYaml
    {
        [YamlMember(Alias = "high-closure-rate-warning-threshold")]
        public uint HighClosureRateWarningThreshold { get; set; }
        
        [YamlMember(Alias = "high-creation-rate-warning-threshold")]
        public uint HighCreationRateWarningThreshold { get; set; }
        
        [YamlMember(Alias = "queue-high-flow-threshold")]
        public uint QueueHighFlowThreshold { get; set; }
        
        [YamlMember(Alias = "queue-low-flow-threshold")]
        public uint QueueLowFlowThreshold { get; set; }
        
        [YamlMember(Alias = "message-redelivery-coefficient")]
        public decimal MessageRedeliveryCoefficient { get; set; }

        [YamlMember(Alias = "socket-usage-coefficient")]
        public decimal SocketUsageCoefficient { get; set; }
        
        [YamlMember(Alias = "runtime-process-usage-coefficient")]
        public decimal RuntimeProcessUsageCoefficient { get; set; }
        
        [YamlMember(Alias = "file-descriptor-usage-warning-coefficient")]
        public decimal FileDescriptorUsageWarningCoefficient { get; set; }
        
        [YamlMember(Alias = "consumer-utilization-warning-coefficient")]
        public decimal ConsumerUtilizationWarningCoefficient { get; set; }
    }
}