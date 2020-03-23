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
    public class DiagnosticsConfigProviderTests
    {
        [Test]
        public void Verify_can_configure_diagnostic_probes()
        {
            var provider = new DiagnosticsConfigProvider();

            var config = provider.Configure(x =>
                                         {
                                             x.SetMessageRedeliveryThresholdCoefficient(0.60M);
                                             x.SetSocketUsageThresholdCoefficient(0.60M);
                                             x.SetConsumerUtilizationThreshold(0.65M);
                                             x.SetQueueHighFlowThreshold(90);
                                             x.SetQueueLowFlowThreshold(10);
                                             x.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                                             x.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                                             x.SetHighClosureRateThreshold(90);
                                             x.SetHighCreationRateThreshold(60);
                                         });
            
            config.Probes.HighClosureRateThreshold.ShouldBe<uint>(90);
            config.Probes.HighCreationRateThreshold.ShouldBe<uint>(60);
            config.Probes.QueueHighFlowThreshold.ShouldBe<uint>(90);
            config.Probes.QueueLowFlowThreshold.ShouldBe<uint>(10);
            config.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(0.60M);
            config.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.60M);
            config.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Probes.ConsumerUtilizationThreshold.ShouldBe(0.65M);

        }

        [Test]
        public void Verify_cannot_configure_diagnostic_probes()
        {
            var provider = new DiagnosticsConfigProvider();

            var config = provider.Configure(x =>
                                         {
                                         });
            
            config.Probes.HighClosureRateThreshold.ShouldBe<uint>(100);
            config.Probes.HighCreationRateThreshold.ShouldBe<uint>(100);
            config.Probes.QueueHighFlowThreshold.ShouldBe<uint>(100);
            config.Probes.QueueLowFlowThreshold.ShouldBe<uint>(20);
            config.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(1M);
            config.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.50M);
            config.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.7M);
            config.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.7M);
            config.Probes.ConsumerUtilizationThreshold.ShouldBe(0.50M);
        }
    }
}