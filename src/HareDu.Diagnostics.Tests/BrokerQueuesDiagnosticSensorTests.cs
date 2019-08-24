namespace HareDu.Diagnostics.Tests
{
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Fakes;
    using NUnit.Framework;
    using Observers;
    using Scanning;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerQueuesDiagnosticSensorTests :
        DiagnosticsTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();
        }

        [Test]
        public void Verify_broker_queues_memory_paging_warning_status()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();
            var report = _container.Resolve<IDiagnosticScanner>()
                .RegisterObserver(new DefaultDiagnosticConsoleLogger())
                .Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Yellow && x.ComponentType == ComponentType.Queue).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
//            Assert.AreEqual(1, results.Count);
        }

    }
}