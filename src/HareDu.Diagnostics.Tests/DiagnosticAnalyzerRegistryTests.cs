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
namespace HareDu.Diagnostics.Tests
{
    using System.Linq;
    using Core.Configuration;
    using Diagnostics.Analyzers;
    using KnowledgeBase;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class DiagnosticAnalyzerRegistryTests
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
            
            var registry = new DiagnosticAnalyzerRegistrar(config.Analyzer, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            
            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache[typeof(AvailableCpuCoresAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageAnalyzer).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out var config);
            
            var registry = new DiagnosticAnalyzerRegistrar(config.Analyzer, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            registry.RegisterAll();
            
            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache[typeof(AvailableCpuCoresAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageAnalyzer).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var registry = new DiagnosticAnalyzerRegistrar(config.Analyzer, new DefaultKnowledgeBaseProvider());

            registry.Register(typeof(TestDiagnosticAnalyzer));
            registry.Register(typeof(TestDiagnosticAnalyzer));

            registry.ObjectCache.Count.ShouldBe(1);
            registry.ObjectCache[typeof(TestDiagnosticAnalyzer).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_the_analyzer_objects_3()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var registry = new DiagnosticAnalyzerRegistrar(config.Analyzer, new DefaultKnowledgeBaseProvider());

            registry.Register<TestDiagnosticAnalyzer>();
            registry.Register<TestDiagnosticAnalyzer>();

            registry.ObjectCache.Count.ShouldBe(1);
            registry.ObjectCache[typeof(TestDiagnosticAnalyzer).GetIdentifier()].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_analyzer_objects_plus_one()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out var config);
            
            var registry = new DiagnosticAnalyzerRegistrar(config.Analyzer, new DefaultKnowledgeBaseProvider());

            registry.RegisterAll();
            registry.Register(typeof(TestDiagnosticAnalyzer));

            registry.ObjectCache.Count.ShouldBe(22);
            registry.ObjectCache[typeof(AvailableCpuCoresAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(BlockedConnectionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelLimitReachedAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ChannelThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(ConsumerUtilizationAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(DiskAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionClosureRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(HighConnectionCreationRateAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MemoryAlarmAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(MessagePagingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(NetworkPartitionAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueGrowthAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueHighFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueLowFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(QueueNoFlowAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RedeliveredMessagesAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(RuntimeProcessLimitAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(SocketDescriptorThrottlingAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnlimitedPrefetchCountAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(UnroutableMessageAnalyzer).GetIdentifier()].ShouldNotBeNull();
            registry.ObjectCache[typeof(TestDiagnosticAnalyzer).GetIdentifier()].ShouldNotBeNull();
        }
    }

    class TestDiagnosticAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public TestDiagnosticAnalyzer(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
        }

        public string Identifier => GetType().GetIdentifier();
        public string Name { get; }
        public string Description { get; }
        public ComponentType ComponentType { get; }
        public DiagnosticAnalyzerCategory Category { get; }
        public DiagnosticAnalyzerStatus Status { get; }
        public DiagnosticAnalyzerResult Execute<T>(T snapshot) => throw new System.NotImplementedException();
    }
}