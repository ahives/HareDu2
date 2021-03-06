namespace HareDu.Snapshotting.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;
    using Observers;
    using Snapshotting;
    using Model;

    [TestFixture]
    public class ClusterSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .Build();
        }

        [Test]
        public async Task Test()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<ClusterSnapshot>()
                .RegisterObserver(new DefaultClusterSnapshotConsoleLogger())
                .TakeSnapshot();

//            var snapshot = resource.Snapshots[0].Select(x => x.Data);
//            Console.WriteLine($"Cluster: {snapshot.ClusterName}");
        }
    }
}