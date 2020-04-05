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
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Core.Extensions;
    using CoreIntegration;
    using HareDu.Diagnostics;
    using HareDu.Diagnostics.Formatting;
    using HareDu.Diagnostics.KnowledgeBase;
    using HareDu.Diagnostics.Probes;
    using HareDu.Diagnostics.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Prometheus;
    using Registration;
    using Snapshotting;
    using Snapshotting.Model;
    using Snapshotting.Registration;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        [Test]
        public async Task Test1()
        {
            var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .AddHareDuSnapshot()
                .AddHareDuDiagnostics()
                .Build();

            var scanner = container.Resolve<IScanner>();

            var lens = container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();

            var result = lens.TakeSnapshot();

            var report = scanner.Scan(result.Snapshot);
            // SendToPrometheus(lens, scanner);

            var formatter = container.Resolve<IDiagnosticReportFormatter>();
            
            string formattedReport = formatter.Format(report);
             
            Console.WriteLine(formattedReport);
        }

        void SendToPrometheus(SnapshotLens<BrokerConnectivitySnapshot> lens, IScanner scanner)
        {
            var server = new MetricServer(hostname: "localhost", port: 1234);
            server.Start();
            
            Gauge gauge = Metrics.CreateGauge("Healthy", "blah, blah, blah");
            
            for (int i = 0; i < 60; i++)
            {
                var result = lens.TakeSnapshot();

                var report = scanner.Scan(result.Snapshot);
                PublishSummary(report.Results
                    .FirstOrDefault(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()), gauge);

                Thread.Sleep(1000);
            }
            
            server.Stop();
        }

        void PublishSummary(ProbeResult result, Gauge gauge)
        {
            if (result.IsNull())
                return;
            
            switch (result.Status)
            {
                case ProbeResultStatus.Unhealthy:
                    gauge.Dec();
                    break;
                case ProbeResultStatus.Healthy:
                    gauge.Inc();
                    break;
                case ProbeResultStatus.Warning:
                    break;
                case ProbeResultStatus.Inconclusive:
                    break;
                case ProbeResultStatus.NA:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        [Test]
        public async Task Test2()
        {
            var services = new ServiceCollection()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .AddHareDuSnapshot()
                .AddHareDuDiagnostics()
                .BuildServiceProvider();
            
            var lens = services.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            var result = lens.TakeSnapshot();
            
            var scanner = services.GetService<IScanner>();

            var report = scanner.Scan(result.Snapshot);

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
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(new BrokerObjectFactory(config));

            var lens = factory.Lens<BrokerConnectivitySnapshot>();
            var result = lens.TakeSnapshot();
            
            IScanner scanner = new Scanner(
                new ScannerFactory(config, new KnowledgeBaseProvider()));

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test4()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(config);
            var lens = factory.Lens<BrokerConnectivitySnapshot>();
            var result = lens.TakeSnapshot();

            var scannerFactory = new ScannerFactory(config, new KnowledgeBaseProvider());
            IScanner scanner = new Scanner(scannerFactory);

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test5()
        {
            // var provider1 = new BrokerConfigProvider();
            // var config1 = provider1.Configure(x =>
            // {
            //     x.ConnectTo("http://localhost:15672");
            //     x.UsingCredentials("guest", "guest");
            // });
            var provider = new HareDuConfigProvider();

            var config1 = provider.Configure(x =>
            {
                x.Broker(y =>
                {
                    y.ConnectTo("http://localhost:15672");
                    y.UsingCredentials("guest", "guest");
                });

                x.Diagnostics(y =>
                {
                    y.Probes(z =>
                    {
                        z.SetMessageRedeliveryThresholdCoefficient(0.60M);
                        z.SetSocketUsageThresholdCoefficient(0.60M);
                        z.SetConsumerUtilizationThreshold(0.65M);
                        z.SetQueueHighFlowThreshold(90);
                        z.SetQueueLowFlowThreshold(10);
                        z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                        z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                        z.SetHighClosureRateThreshold(90);
                        z.SetHighCreationRateThreshold(60);
                    });
                });
            });
            
            var factory1 = new SnapshotFactory(config1);
            var lens = factory1.Lens<BrokerConnectivitySnapshot>();

            var result = lens.TakeSnapshot();

            var provider2 = new DiagnosticsConfigProvider();

            var config = provider2.Configure(x =>
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
            
            var factory2 = new ScannerFactory(config1, new KnowledgeBaseProvider());
            IScanner scanner = new Scanner(factory2);

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
    }
}