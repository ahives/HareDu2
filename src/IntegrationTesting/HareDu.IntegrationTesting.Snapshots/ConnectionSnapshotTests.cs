namespace HareDu.IntegrationTesting.Snapshots
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;
    using Observers;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class ConnectionSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .Build();
        }

        [Test]
        public async Task Test1()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>()
                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .TakeSnapshot();
        }

        [Test]
        public async Task Test2()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>()
                .TakeSnapshot();
        }
    }
}