namespace HareDu.Snapshotting.Tests
{
    using Autofac;
    using NUnit.Framework;
    using Observers;

    [TestFixture]
    public class QueueSnapshotTests :
        SnapshotTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
//            builder.RegisterModule<MassTransitModule>();

            _container = builder.Build();
        }

        [Test]
        public void Test()
        {
            var queue = Client
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new DefaultQueueSnapshotConsoleLogger())
                .Take();
        }
    }
}