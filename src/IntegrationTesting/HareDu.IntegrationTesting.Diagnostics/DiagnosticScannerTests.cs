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
    using Snapshotting.Observers;
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
                .Snapshot<BrokerConnectivity>()
//                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                // .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .Execute();
            
            var scanner = container.Resolve<IDiagnosticScanner>();

            var snapshot = resource.Timeline.MostRecent().Snapshot;
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
                .AddHareDuDiagnostics($"{TestContext.CurrentContext.TestDirectory}/config.yaml")
                .BuildServiceProvider();
            
            var resource = services.GetService<ISnapshotFactory>()
                .Snapshot<BrokerConnectivity>()
                .Execute();
            
            var scanner = services.GetService<IDiagnosticScanner>();

            var snapshot = resource.Timeline.MostRecent().Snapshot;
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
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet($"{Directory.GetCurrentDirectory()}/config.yaml", out HareDuConfig config);

            var comm = new BrokerCommunication();
            var factory = new SnapshotFactory(new BrokerObjectFactory(comm.GetClient(config.Broker)));

            var resource = factory.Snapshot<BrokerConnectivity>().Execute();
            
            IDiagnosticScanner scanner = new DiagnosticScanner(
                new DiagnosticFactory(config.Diagnostics, new DefaultKnowledgeBaseProvider()));

            var snapshot = resource.Timeline.MostRecent().Snapshot;
            var report = scanner.Scan(snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
    }
}