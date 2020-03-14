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
namespace HareDu.Diagnostics.Tests.Analyzers
{
    using System;
    using System.IO;
    using Autofac;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Analyzers;
    using Diagnostics.Registration;
    using Extensions;
    using Fakes;
    using Formatting;
    using KnowledgeBase;
    using NUnit.Framework;
    using Snapshotting.Model;

    [TestFixture]
    public class ChannelThrottlingReportAnalyzerTests
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
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot3();
            IScanAnalyzerFactory factory = _container.Resolve<IScanAnalyzerFactory>();

            var summary = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot)
                .Analyze(factory, typeof(ThrottledChannelsScanAnalyzer).GetIdentifier());
            
            for (int i = 0; i < summary.Count; i++)
            {
                Assert.AreEqual(30.0, summary[i].Healthy.Percentage);
                Assert.AreEqual(70.0, summary[i].Unhealthy.Percentage);
                Assert.AreEqual(0.0, summary[i].Warning.Percentage);
                Assert.AreEqual(0.0, summary[i].Inconclusive.Percentage);
//                Console.WriteLine(summary[i].Identifier);
//                Console.WriteLine($"\t{summary[i].Green.Percentage}% green");
//                Console.WriteLine($"\t{summary[i].Red.Percentage}% red");
//                Console.WriteLine($"\t{summary[i].Yellow.Percentage}% yellow");
//                Console.WriteLine($"\t{summary[i].Inconclusive.Percentage}% inconclusive");
            }
        }
        
        [Test]
        public void Test2()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot3();
            
            var report = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);

            var summary = _container.Resolve<IScanAnalyzer>()
                .Analyze(report);

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