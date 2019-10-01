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
namespace HareDu.Diagnostics.Configuration
{
    using YamlDotNet.Serialization;

    public interface DiagnosticAnalyzerConfig
    {
        [YamlMember(Alias = "high-closure-rate-warning-threshold")]
        int HighClosureRateWarningThreshold { get; }
        
        [YamlMember(Alias = "high-creation-rate-warning-threshold")]
        int HighCreationRateWarningThreshold { get; }
        
        [YamlMember(Alias = "message-redelivery-coefficient")]
        decimal MessageRedeliveryCoefficient { get; }

        [YamlMember(Alias = "socket-usage-coefficient")]
        decimal SocketUsageCoefficient { get; }
        
        [YamlMember(Alias = "runtime-process-usage-coefficient")]
        decimal RuntimeProcessUsageCoefficient { get; }
        
        [YamlMember(Alias = "file-descriptor-usage-warning-coefficient")]
        decimal FileDescriptorUsageWarningCoefficient { get; }
        
        [YamlMember(Alias = "consumer-utilization-warning-coefficient")]
        decimal ConsumerUtilizationWarningCoefficient { get; }
    }
}