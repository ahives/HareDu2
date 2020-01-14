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
namespace HareDu.Diagnostics.Tests.Registration
{
    using Core.Configuration;
    using Diagnostics.Probes;
    using Diagnostics.Registration;
    using Extensions;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class DiagnosticProbeRegistrarTests
    {
        // [Test]
        // public void Verify_will_throw_when_param_missing()
        // {
        //     var registry = new DiagnosticAnalyzerRegistry(new ConfigurationProvider());
        //
        //     Should.Throw<TargetInvocationException>(() =>
        //         registry.RegisterAll($"{TestContext.CurrentContext.TestDirectory}/config.yaml", null));
        // }

        [Test]
        public void Verify_will_register_all_analyzer_objects()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out var config);
            
            var registry = new DiagnosticProbeRegistrar(config.Diagnostics, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            
            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache[typeof(AvailableCpuCoresProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageProbe).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out var config);
            
            var registry = new DiagnosticProbeRegistrar(config.Diagnostics, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            registry.RegisterAll();
            
            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache[typeof(AvailableCpuCoresProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageProbe).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var registry = new DiagnosticProbeRegistrar(config.Diagnostics, new DefaultKnowledgeBaseProvider());

            registry.Register(typeof(TestDiagnosticProbe));
            registry.Register(typeof(TestDiagnosticProbe));

            registry.ObjectCache.Count.ShouldBe(1);
            registry.ObjectCache[typeof(TestDiagnosticProbe).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_3()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var registry = new DiagnosticProbeRegistrar(config.Diagnostics, new DefaultKnowledgeBaseProvider());

            registry.Register<TestDiagnosticProbe>();
            registry.Register<TestDiagnosticProbe>();

            registry.ObjectCache.Count.ShouldBe(1);
            registry.ObjectCache[typeof(TestDiagnosticProbe).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_analyzer_objects_plus_one()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out var config);
            
            var registry = new DiagnosticProbeRegistrar(config.Diagnostics, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            registry.Register(typeof(TestDiagnosticProbe));

            registry.ObjectCache.Count.ShouldBe(22);
            registry.ObjectCache[typeof(AvailableCpuCoresProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageProbe).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(TestDiagnosticProbe).GetIdentifier()].ShouldNotBeNull();
        }
    }

    class TestDiagnosticProbe :
        BaseDiagnosticProbe,
        IDiagnosticProbe
    {
        public TestDiagnosticProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
        }

        public string Identifier => GetType().GetIdentifier();
        public string Name { get; }
        public string Description { get; }
        public ComponentType ComponentType { get; }
        public DiagnosticProbeCategory Category { get; }
        public DiagnosticProbeStatus Status { get; }
        public DiagnosticProbeResult Execute<T>(T snapshot) => throw new System.NotImplementedException();
    }
}