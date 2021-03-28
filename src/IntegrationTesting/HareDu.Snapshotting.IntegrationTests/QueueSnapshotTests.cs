namespace HareDu.Snapshotting.IntegrationTests
{
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using CoreIntegration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Observers;
    using Snapshotting;
    using Model;

    [TestFixture]
    public class QueueSnapshotTests
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
        public void Test()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerQueuesSnapshot>()
                .RegisterObserver(new DefaultQueueSnapshotConsoleLogger())
                // .RegisterObserver(new BrokerQueuesJsonExporter())
                .TakeSnapshot();
            
//            resource.Snapshots.Export();
        }

        [Test]
        public void Test1()
        {
            var provider = new YamlFileConfigProvider();
            
            if (provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml", out HareDuConfig config))
            {
                var brokerFactory = new BrokerObjectFactory(config);
                var factory = new SnapshotFactory(brokerFactory);

                var lens = factory
                    .Lens<BrokerQueuesSnapshot>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .TakeSnapshot();
            }
        }

        [Test]
        public void Test2()
        {
            var provider = new YamlFileConfigProvider();
            
            if (provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml", out HareDuConfig config))
            {
                var brokerFactory = new BrokerObjectFactory(config);
                var factory = new SnapshotFactory(brokerFactory);

                var lens = factory
                    .Lens<BrokerQueuesSnapshot>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .TakeSnapshot();
            }
        }

        // [Test]
        // public void Test3()
        // {
        //     var configurationProvider = new ConfigurationProvider();
        //     
        //     if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml", out HareDuConfig config))
        //     {
        //         var comm = new BrokerCommunication();
        //         var factory = new SnapshotFactory(new BrokerObjectFactory(comm.GetClient(config.Broker)));
        //
        //         var resource = factory
        //             .Snapshot<BrokerQueues>()
        //             .RegisterObserver(new BrokerQueuesJsonExporter())
        //             .Execute();
        //     }
        // }
        //
        // [Test]
        // public void Test4()
        // {
        //     var configurationProvider = new ConfigurationProvider();
        //     
        //     if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml", out HareDuConfig config))
        //     {
        //         var comm = new BrokerCommunication();
        //         var factory = new SnapshotFactory(comm.GetClient(config.Broker));
        //
        //         var resource = factory
        //             .Snapshot<BrokerQueues>()
        //             .RegisterObserver(new BrokerQueuesJsonExporter())
        //             .Execute();
        //     }
        // }
        //
        // [Test]
        // public void Test5()
        // {
        //     var factory = new SnapshotFactory(x =>
        //     {
        //         x.ConnectTo("http://localhost:15672");
        //         x.UsingCredentials("guest", "guest");
        //     });
        //
        //     var resource = factory
        //         .Snapshot<BrokerQueues>()
        //         .RegisterObserver(new BrokerQueuesJsonExporter())
        //         .Execute();
        // }
        //
        // [Test]
        // public void Test6()
        // {
        //     var factory = new SnapshotFactory();
        //
        //     var resource = factory
        //         .Snapshot<BrokerQueues>()
        //         .RegisterObserver(new BrokerQueuesJsonExporter())
        //         .Execute();
        // }

        [Test]
        public void Test7()
        {
            var services = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .BuildServiceProvider();

            var resource = services.GetService<ISnapshotFactory>()
                .Lens<BrokerQueuesSnapshot>()
                .RegisterObserver(new BrokerQueuesJsonExporter())
                .TakeSnapshot();
        }
    }
}