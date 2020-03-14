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
    using Autofac;
    using Core.Configuration;
    using Diagnostics;
    using Diagnostics.Analyzers;
    using Diagnostics.Extensions;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Tests.Fakes;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class QueueNoFlowReportAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            // builder.RegisterModule<HareDuAnalyticsModule>();
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

            builder.RegisterType<ScanAnalyzerFactory>()
                .As<IScanAnalyzerFactory>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScanner>()
                .As<IDiagnosticScanner>()
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
        public void Test1()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IScanAnalyzerFactory factory = _container.Resolve<IScanAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(QueueNoFlowScanAnalyzer).FullName);
            
            summary.ShouldNotBeNull();
            summary.Count.ShouldBe(1);
            summary[0].Healthy.Total.ShouldBe<uint>(3);
            summary[0].Healthy.Percentage.ShouldBe(37.5M);
            summary[0].Unhealthy.Total.ShouldBe<uint>(5);
            summary[0].Unhealthy.Percentage.ShouldBe(62.5M);
            summary[0].Warning.Total.ShouldBe<uint>(0);
            summary[0].Warning.Percentage.ShouldBe(0);
            summary[0].Inconclusive.Total.ShouldBe<uint>(0);
            summary[0].Inconclusive.Percentage.ShouldBe(0);
        }
        
        [Test]
        public void Test2()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IScanAnalyzerFactory factory = _container.Resolve<IScanAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(QueueNoFlowScanAnalyzer));
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Id);
                Console.WriteLine($"\t{summary[i].Healthy.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Unhealthy.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Warning.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test3()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IScanAnalyzerFactory factory = _container.Resolve<IScanAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze<QueueNoFlowScanAnalyzer>(factory);
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Id);
                Console.WriteLine($"\t{summary[i].Healthy.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Unhealthy.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Warning.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test4()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IScanAnalyzer analyzer = new QueueNoFlowScanAnalyzer();
            
            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(analyzer);
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Id);
                Console.WriteLine($"\t{summary[i].Healthy.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Unhealthy.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Warning.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
    }
}