namespace HareDu.IntegrationTesting.Diagnostics
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
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
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Observers;
    using Prometheus;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        [Test]
        public async Task Test1()
        {
            var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();

            var scanner = container.Resolve<IScanner>();

            var lens = container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            
            var result = await lens.TakeSnapshot();
            
            var report = scanner.Scan(result.Snapshot);

            var formatter = container.Resolve<IDiagnosticReportFormatter>();
            
            string formattedReport = formatter.Format(report);
             
            Console.WriteLine(formattedReport);
        }

        [Test]
        public async Task Test2()
        {
            var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();

            var scanner = container.Resolve<IScanner>();

            var lens = container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();

            await SendToPrometheus(lens, scanner);
        }

        async Task SendToPrometheus(SnapshotLens<BrokerConnectivitySnapshot> lens, IScanner scanner)
        {
            // var pusher = new MetricPusher(new MetricPusherOptions
            // {
            //     Endpoint = "https://pushgateway.example.org:9091/metrics",
            //     Job = "HareDuDiagnostics"
            // });
            //
            // pusher.Start();
            var server = new MetricServer(hostname: "localhost", port: 1234);
            server.Start();
            
            Gauge gauge = Metrics.CreateGauge("Healthy", "blah, blah, blah",
                new GaugeConfiguration());
            scanner.RegisterObserver(new DefaultDiagnosticConsoleLogger());
            for (int i = 0; i < 10; i++)
            {
                var result = await lens.TakeSnapshot();

                var report = scanner.Scan(result.Snapshot);
                PublishSummary(report.Results
                    .FirstOrDefault(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()), gauge);

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
            using (var stream = new MemoryStream())
            {
                await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);

                var text = Encoding.UTF8.GetString(stream.ToArray());

                Console.WriteLine(text);
            }
            
            server.Stop();
            // pusher.Stop();
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
        public async Task Test3()
        {
            var services = new ServiceCollection()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .BuildServiceProvider();
            
            var lens = services.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            var result = await lens.TakeSnapshot();
            
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
        public async Task Test4()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(new BrokerObjectFactory(config));

            var lens = factory.Lens<BrokerConnectivitySnapshot>();
            var result = await lens.TakeSnapshot();
            
            IScanner scanner = new Scanner(
                new ScannerFactory(config, new KnowledgeBaseProvider()));

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test5()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

            var factory = new SnapshotFactory(config);
            var lens = factory.Lens<BrokerConnectivitySnapshot>();
            var result = await lens.TakeSnapshot();

            var scannerFactory = new ScannerFactory(config, new KnowledgeBaseProvider());
            IScanner scanner = new Scanner(scannerFactory);

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
        
        [Test]
        public async Task Test6()
        {
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
                        z.SetHighConnectionClosureRateThreshold(90);
                        z.SetHighConnectionCreationRateThreshold(60);
                    });
                });
            });
            
            var factory1 = new SnapshotFactory(config1);
            var lens = factory1.Lens<BrokerConnectivitySnapshot>();

            var result = await lens.TakeSnapshot();
            
            var factory2 = new ScannerFactory(config1, new KnowledgeBaseProvider());
            IScanner scanner = new Scanner(factory2);

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
    }
}