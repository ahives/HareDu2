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
namespace HareDu.Core.Tests.Configuration
{
    using Core.Configuration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ConfigurationProviderTests
    {
        [Test]
        public void Test()
        {
            var validator = new HareDuConfigValidator();
            var provider = new YamlFileConfigProvider(validator);

            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            
            provider.TryGet(path, out var config).ShouldBeTrue();
            config.Broker.Url.ShouldBe("http://localhost:15672");
            config.Broker.Credentials.Username.ShouldBe("guest");
            config.Broker.Credentials.Password.ShouldBe("guest");
            config.Diagnostics.Probes.HighClosureRateThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.HighCreationRateThreshold.ShouldBe<uint>(60);
            config.Diagnostics.Probes.QueueHighFlowThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.QueueLowFlowThreshold.ShouldBe<uint>(10);
            config.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.ConsumerUtilizationThreshold.ShouldBe(0.65M);
        }
        
        [Test]
        public void Test2()
        {
            var validator = new HareDuConfigValidator();
            var provider = new YamlConfigProvider(validator);

            string text = @"---
  broker:
      url:  http://localhost:15672
      username: guest
      password: guest
  diagnostics:
    probes:
      high-closure-rate-threshold:  90
      high-creation-rate-threshold: 60
      queue-high-flow-threshold:  90
      queue-low-flow-threshold: 10
      message-redelivery-threshold-coefficient: 0.60
      socket-usage-threshold-coefficient: 0.60
      runtime-process-usage-threshold-coefficient:  0.65
      file-descriptor-usage-threshold-coefficient:  0.65
      consumer-utilization-threshold: 0.65
...";
            
            provider.TryGet(text, out var config).ShouldBeTrue();
            config.Broker.Url.ShouldBe("http://localhost:15672");
            config.Broker.Credentials.Username.ShouldBe("guest");
            config.Broker.Credentials.Password.ShouldBe("guest");
            config.Diagnostics.Probes.HighClosureRateThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.HighCreationRateThreshold.ShouldBe<uint>(60);
            config.Diagnostics.Probes.QueueHighFlowThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.QueueLowFlowThreshold.ShouldBe<uint>(10);
            config.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.ConsumerUtilizationThreshold.ShouldBe(0.65M);
        }
    }
}