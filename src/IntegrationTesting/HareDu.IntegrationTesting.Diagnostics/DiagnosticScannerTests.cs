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
namespace HareDu.IntegrationTesting.Diagnostics
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Configuration;
    using CoreIntegration;
    using HareDu.Diagnostics;
    using HareDu.Diagnostics.Formatting;
    using HareDu.Diagnostics.KnowledgeBase;
    using HareDu.Diagnostics.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Registration;
    using Snapshotting;
    using Snapshotting.Extensions;
    using Snapshotting.Registration;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        [Test]
        public async Task Test1()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuDiagnosticsModule>();

            var container = builder.Build();

            var resource = container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivity>()
//                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                // .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .TakeSnapshot();
            
            var scanner = container.Resolve<IDiagnosticScanner>();

            var snapshot = resource.History.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = container.Resolve<IDiagnosticReportFormatter>();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
            
//            for (int i = 0; i < report.Results.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", report.Results[i].ComponentIdentifier, report.Results[i].Status);
//                
//                if (report.Results[i].Status == DiagnosticStatus.Red)
//                {
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Reason);
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Remediation);
//                }
//            }
        }

        [Test]
        public async Task Test2()
        {
            var services = new ServiceCollection()
                .AddHareDuDiagnostics($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .BuildServiceProvider();
            
            var resource = services.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivity>()
                .TakeSnapshot();
            
            var scanner = services.GetService<IDiagnosticScanner>();

            var snapshot = resource.History.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = services.GetService<IDiagnosticReportFormatter>();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
            
//            for (int i = 0; i < report.Results.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", report.Results[i].ComponentIdentifier, report.Results[i].Status);
//                
//                if (report.Results[i].Status == DiagnosticStatus.Red)
//                {
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Reason);
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Remediation);
//                }
//            }
        }
        
        [Test]
        public async Task Test3()
        {
            var validator = new HareDuConfigValidator();
            var configProvider = new YamlFileConfigProvider(validator);
            configProvider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(new BrokerObjectFactory(config.Broker));

            var resource = factory.Lens<BrokerConnectivity>().TakeSnapshot();
            
            IDiagnosticScanner scanner = new DiagnosticScanner(
                new DiagnosticFactory(config.Diagnostics, new KnowledgeBaseProvider()));

            var snapshot = resource.History.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test4()
        {
            var validator = new HareDuConfigValidator();
            var configProvider = new YamlFileConfigProvider(validator);
            configProvider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(config.Broker);
            var resource = factory.Lens<BrokerConnectivity>().TakeSnapshot();

            IDiagnosticScanner scanner = new DiagnosticScanner(config.Diagnostics);

            var snapshot = resource.History.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test5()
        {
            var provider1 = new BrokerConfigProvider();
            var config1 = provider1.Configure(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.UsingCredentials("guest", "guest");
            });
            var factory1 = new SnapshotFactory(config1);
            var resource = factory1.Lens<BrokerConnectivity>().TakeSnapshot();

            var provider2 = new DiagnosticsConfigProvider();

            var config = provider2.Configure(x =>
            {
                x.SetMessageRedeliveryCoefficient(0.60M);
                x.SetSocketUsageCoefficient(0.60M);
                x.SetConsumerUtilizationWarningCoefficient(0.65M);
                x.SetQueueHighFlowThreshold(90);
                x.SetQueueLowFlowThreshold(10);
                x.SetRuntimeProcessUsageCoefficient(0.65M);
                x.SetFileDescriptorUsageWarningCoefficient(0.65M);
                x.SetHighClosureRateWarningThreshold(90);
                x.SetHighCreationRateWarningThreshold(60);
            });
            
            var factory2 = new DiagnosticFactory(config, new KnowledgeBaseProvider());
            IDiagnosticScanner scanner = new DiagnosticScanner(factory2);

            var snapshot = resource.History.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
    }
}