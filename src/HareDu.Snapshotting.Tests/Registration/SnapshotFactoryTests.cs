namespace HareDu.Snapshotting.Tests.Registration
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Fakes;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class SnapshotFactoryTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();
        }

        [Test]
        public async Task Verify_can_return_BrokerConnection_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var lens = factory.Lens<BrokerConnectivitySnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<BrokerConnectivitySnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_BrokerQueues_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var lens = factory.Lens<BrokerQueuesSnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<BrokerQueuesSnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_Cluster_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var lens = factory.Lens<ClusterSnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<ClusterSnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_new_snapshots()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var lens = factory
                .Register(new FakeHareDuSnapshot1Impl())
                .Lens<FakeHareDuSnapshot1>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<FakeHareDuSnapshot1>>();
        }

        [Test]
        public void Verify_snapshot_not_implemented_does_not_throw()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var lens = factory.Lens<FakeHareDuSnapshot2>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<FakeHareDuSnapshot2>>();
        }
    }
}