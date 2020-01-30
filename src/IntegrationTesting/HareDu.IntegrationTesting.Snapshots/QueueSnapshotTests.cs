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
namespace HareDu.IntegrationTesting.Snapshots
{
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Configuration;
    using CoreIntegration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Registration;
    using Snapshotting;
    using Snapshotting.Exporters;
    using Snapshotting.Observers;
    using Snapshotting.Registration;

    [TestFixture]
    public class QueueSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuSnapshottingModule>();

            _container = builder.Build();
        }

        [Test]
        public void Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new DefaultQueueSnapshotConsoleLogger())
                // .RegisterObserver(new BrokerQueuesJsonExporter())
                .Execute();
            
//            resource.Snapshots.Export();
        }

        [Test]
        public void Test1()
        {
            var configurationProvider = new ConfigurationProvider();
            
            if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
            {
                var comm = new BrokerCommunication();
                var client = comm.GetClient(config.Broker);

                var brokerObjectRegistrar = new BrokerObjectRegistrar();
                brokerObjectRegistrar.RegisterAll(client);

                var brokerFactory = new BrokerObjectFactory(client, brokerObjectRegistrar);
                var instanceCreator = new SnapshotInstanceCreator(brokerFactory);
                var snapshotObjectRegistrar = new SnapshotObjectRegistrar(instanceCreator);

                snapshotObjectRegistrar.RegisterAll();

                var factory = new SnapshotFactory(brokerFactory, snapshotObjectRegistrar);

                var resource = factory
                    .Snapshot<BrokerQueues>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .Execute();
            }
        }

        [Test]
        public void Test2()
        {
            var configurationProvider = new ConfigurationProvider();
            
            if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
            {
                var comm = new BrokerCommunication();
                var client = comm.GetClient(config.Broker);

                var brokerObjectRegistrar = new BrokerObjectRegistrar();
                brokerObjectRegistrar.RegisterAll(client);

                var brokerFactory = new BrokerObjectFactory(client, brokerObjectRegistrar);
                var instanceCreator = new SnapshotInstanceCreator(brokerFactory);
                var snapshotObjectRegistry = new SnapshotObjectRegistrar(instanceCreator);

                snapshotObjectRegistry.RegisterAll();

                var factory = new SnapshotFactory(brokerFactory, snapshotObjectRegistry);

                var resource = factory
                    .Snapshot<BrokerQueues>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .Execute();
            }
        }

        // [Test]
        // public void Test3()
        // {
        //     var configurationProvider = new ConfigurationProvider();
        //     
        //     if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
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
        //     if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
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
                .AddHareDuSnapshotting($"{TestContext.CurrentContext.TestDirectory}/config.yaml")
                .BuildServiceProvider();

            var resource = services.GetService<ISnapshotFactory>()
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new BrokerQueuesJsonExporter())
                .Execute();
        }
    }
}