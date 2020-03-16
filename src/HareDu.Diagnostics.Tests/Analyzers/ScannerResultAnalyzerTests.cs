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
namespace HareDu.Analytics.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Autofac;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.Extensions;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Probes;
    using Diagnostics.Registration;
    using Diagnostics.Tests.Fakes;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ScannerResultAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IFileConfigProvider>();
                    string path = $"{Directory.GetCurrentDirectory()}/haredu_3.yaml";

                    configProvider.TryGet(path, out var config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();

                    return new ScannerFactory(config.Diagnostics, knowledgeBaseProvider);
                })
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();

            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IConfigProvider>()
                .SingleInstance();

            builder.RegisterType<HareDuConfigValidator>()
                .As<IConfigValidator>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }
        
        [Test]
        public void Verify_can_analyze_by_component_type()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var summary = _container.Resolve<IScanner>()
                .Scan(snapshot)
                .Analyze(_container.Resolve<IScannerResultAnalyzer>(), x => x.ComponentType.ToString());
            
            summary.ShouldNotBeNull();
            summary.Count.ShouldBe(2);
            
            summary.Any(x => x.Id == "Queue").ShouldBeTrue();

            var queueSummary = summary
                .SingleOrDefault(x => x.Id == "Queue");
            queueSummary.ShouldNotBeNull();
            queueSummary.Healthy.Total.ShouldBe<uint>(24);
            Decimal.Round(queueSummary.Healthy.Percentage, 2).ShouldBe(42.86M);
            queueSummary.Unhealthy.Total.ShouldBe<uint>(32);
            Decimal.Round(queueSummary.Unhealthy.Percentage, 2).ShouldBe(57.14M);
            queueSummary.Warning.Total.ShouldBe<uint>(0);
            queueSummary.Warning.Percentage.ShouldBe(0);
            queueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            queueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == "Exchange").ShouldBeTrue();

            var exchangeSummary = summary
                .SingleOrDefault(x => x.Id == "Exchange");
            exchangeSummary.ShouldNotBeNull();
            exchangeSummary.Healthy.Total.ShouldBe<uint>(0);
            exchangeSummary.Healthy.Percentage.ShouldBe(0);
            exchangeSummary.Unhealthy.Total.ShouldBe<uint>(1);
            exchangeSummary.Unhealthy.Percentage.ShouldBe(100);
            exchangeSummary.Warning.Total.ShouldBe<uint>(0);
            exchangeSummary.Warning.Percentage.ShouldBe(0);
            exchangeSummary.Inconclusive.Total.ShouldBe<uint>(0);
            exchangeSummary.Inconclusive.Percentage.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_analyze_by_parent_component()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var summary = _container.Resolve<IScanner>()
                .Scan(snapshot)
                .Analyze(_container.Resolve<IScannerResultAnalyzer>(), x => x.ParentComponentId);
            
            summary.Count.ShouldBe(2);
            
            summary.Any(x => x.Id == "Cluster 1").ShouldBeTrue();

            var clusterSummary = summary
                .SingleOrDefault(x => x.Id == "Cluster 1");
            clusterSummary.ShouldNotBeNull();
            clusterSummary.Healthy.Total.ShouldBe<uint>(0);
            clusterSummary.Healthy.Percentage.ShouldBe(0);
            clusterSummary.Unhealthy.Total.ShouldBe<uint>(1);
            clusterSummary.Unhealthy.Percentage.ShouldBe(100);
            clusterSummary.Warning.Total.ShouldBe<uint>(0);
            clusterSummary.Warning.Percentage.ShouldBe(0);
            clusterSummary.Inconclusive.Total.ShouldBe<uint>(0);
            clusterSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == "Node0").ShouldBeTrue();

            var nodeSummary = summary
                .SingleOrDefault(x => x.Id == "Node0");
            nodeSummary.ShouldNotBeNull();
            nodeSummary.Healthy.Total.ShouldBe<uint>(24);
            Decimal.Round(nodeSummary.Healthy.Percentage, 2).ShouldBe(42.86M);
            nodeSummary.Unhealthy.Total.ShouldBe<uint>(32);
            Decimal.Round(nodeSummary.Unhealthy.Percentage, 2).ShouldBe(57.14M);
            nodeSummary.Warning.Total.ShouldBe<uint>(0);
            nodeSummary.Warning.Percentage.ShouldBe(0);
            nodeSummary.Inconclusive.Total.ShouldBe<uint>(0);
            nodeSummary.Inconclusive.Percentage.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_analyze_by_probe()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var summary = _container.Resolve<IScanner>()
                .Scan(snapshot)
                .Analyze(_container.Resolve<IScannerResultAnalyzer>(), x => x.Id);
            
            summary.Count.ShouldBe(8);
            
            summary.Any(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier()).ShouldBeTrue();

            var unroutableSummary = summary
                .SingleOrDefault(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier());
            unroutableSummary.ShouldNotBeNull();
            unroutableSummary.Healthy.Total.ShouldBe<uint>(0);
            unroutableSummary.Healthy.Percentage.ShouldBe(0);
            unroutableSummary.Unhealthy.Total.ShouldBe<uint>(1);
            unroutableSummary.Unhealthy.Percentage.ShouldBe(100);
            unroutableSummary.Warning.Total.ShouldBe<uint>(0);
            unroutableSummary.Warning.Percentage.ShouldBe(0);
            unroutableSummary.Inconclusive.Total.ShouldBe<uint>(0);
            unroutableSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(MessagePagingProbe).GetIdentifier()).ShouldBeTrue();

            var memoryPagedOutSummary = summary
                .SingleOrDefault(x => x.Id == typeof(MessagePagingProbe).GetIdentifier());
            memoryPagedOutSummary.ShouldNotBeNull();
            memoryPagedOutSummary.Healthy.Total.ShouldBe<uint>(5);
            memoryPagedOutSummary.Healthy.Percentage.ShouldBe(62.5M);
            memoryPagedOutSummary.Unhealthy.Total.ShouldBe<uint>(3);
            memoryPagedOutSummary.Unhealthy.Percentage.ShouldBe(37.5M);
            memoryPagedOutSummary.Warning.Total.ShouldBe<uint>(0);
            memoryPagedOutSummary.Warning.Percentage.ShouldBe(0);
            memoryPagedOutSummary.Inconclusive.Total.ShouldBe<uint>(0);
            memoryPagedOutSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()).ShouldBeTrue();

            var redeliveredMessagesSummary = summary
                .SingleOrDefault(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier());
            redeliveredMessagesSummary.ShouldNotBeNull();
            redeliveredMessagesSummary.Healthy.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Healthy.Percentage.ShouldBe(0);
            redeliveredMessagesSummary.Unhealthy.Total.ShouldBe<uint>(8);
            redeliveredMessagesSummary.Unhealthy.Percentage.ShouldBe(100);
            redeliveredMessagesSummary.Warning.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Warning.Percentage.ShouldBe(0);
            redeliveredMessagesSummary.Inconclusive.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier()).ShouldBeTrue();

            var noFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier());
            noFlowQueueSummary.ShouldNotBeNull();
            noFlowQueueSummary.Healthy.Total.ShouldBe<uint>(3);
            noFlowQueueSummary.Healthy.Percentage.ShouldBe(37.5M);
            noFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(5);
            noFlowQueueSummary.Unhealthy.Percentage.ShouldBe(62.5M);
            noFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            noFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            noFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            noFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier()).ShouldBeTrue();

            var queueGrowthSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier());
            queueGrowthSummary.ShouldNotBeNull();
            queueGrowthSummary.Healthy.Total.ShouldBe<uint>(8);
            queueGrowthSummary.Healthy.Percentage.ShouldBe(100);
            queueGrowthSummary.Unhealthy.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Unhealthy.Percentage.ShouldBe(0);
            queueGrowthSummary.Warning.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Warning.Percentage.ShouldBe(0);
            queueGrowthSummary.Inconclusive.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier()).ShouldBeTrue();

            var lowFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier());
            lowFlowQueueSummary.ShouldNotBeNull();
            lowFlowQueueSummary.Healthy.Total.ShouldBe<uint>(1);
            lowFlowQueueSummary.Healthy.Percentage.ShouldBe(12.5M);
            lowFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(7);
            lowFlowQueueSummary.Unhealthy.Percentage.ShouldBe(87.5M);
            lowFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            lowFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            lowFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            lowFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier()).ShouldBeTrue();

            var highFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier());
            highFlowQueueSummary.ShouldNotBeNull();
            highFlowQueueSummary.Healthy.Total.ShouldBe<uint>(7);
            highFlowQueueSummary.Healthy.Percentage.ShouldBe(87.5M);
            highFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(1);
            highFlowQueueSummary.Unhealthy.Percentage.ShouldBe(12.5M);
            highFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            highFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            highFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            highFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier()).ShouldBeTrue();

            var consumerUtilizationSummary = summary
                .SingleOrDefault(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier());
            consumerUtilizationSummary.ShouldNotBeNull();
            consumerUtilizationSummary.Healthy.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Healthy.Percentage.ShouldBe(0);
            consumerUtilizationSummary.Unhealthy.Total.ShouldBe<uint>(8);
            consumerUtilizationSummary.Unhealthy.Percentage.ShouldBe(100);
            consumerUtilizationSummary.Warning.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Warning.Percentage.ShouldBe(0);
            consumerUtilizationSummary.Inconclusive.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Inconclusive.Percentage.ShouldBe(0);
        }
    }
}