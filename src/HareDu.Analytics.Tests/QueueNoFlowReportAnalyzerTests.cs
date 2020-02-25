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
    using Analyzers;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Fakes;
    using NUnit.Framework;
    using Registration;
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
                    string path = $"{Directory.GetCurrentDirectory()}/haredu.yaml";

                    configProvider.TryGet(path, out var config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();

                    return new DiagnosticFactory(config.Diagnostics, knowledgeBaseProvider);
                })
                .As<IDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = x.Resolve<IAnalyticsRegistry>();
                    
                    registrar.RegisterAll();
                    
                    return new DiagnosticReportAnalyzerFactory(registrar.Cache);
                })
                .As<IDiagnosticReportAnalyzerFactory>()
                .SingleInstance();

            builder.RegisterType<AnalyticsRegistry>()
                .As<IAnalyticsRegistry>()
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
            IDiagnosticReportAnalyzerFactory factory = _container.Resolve<IDiagnosticReportAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(QueueNoFlowReportAnalyzer).GetIdentifier());
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Identifier);
                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test2()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IDiagnosticReportAnalyzerFactory factory = _container.Resolve<IDiagnosticReportAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(QueueNoFlowReportAnalyzer));
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Identifier);
                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test3()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IDiagnosticReportAnalyzerFactory factory = _container.Resolve<IDiagnosticReportAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze<QueueNoFlowReportAnalyzer>(factory);
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Identifier);
                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test4()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            IDiagnosticReportAnalyzer analyzer = new QueueNoFlowReportAnalyzer();
            
            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(analyzer);
            
            for (int i = 0; i < summary.Count; i++)
            {
                Console.WriteLine(summary[i].Identifier);
                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
    }
}