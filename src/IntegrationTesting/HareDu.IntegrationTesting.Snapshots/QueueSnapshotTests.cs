// Copyright 2013-2019 Albert L. Hives
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
                .RegisterObserver(new BrokerQueuesJsonExporter())
                .Execute();
            
//            resource.Snapshots.Export();
        }

        [Test]
        public void Test1()
        {
            var configurationProvider = new ConfigurationProvider();
            
            if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
            {
                var comm = new BrokerConnectionClient();
                var client = comm.Create(config.Broker);

                var brokerObjectRegistrar = new BrokerObjectRegistration();
                brokerObjectRegistrar.RegisterAll(client);

                var registration = new SnapshotRegistration();
                var brokerFactory = new BrokerObjectFactory(client, brokerObjectRegistrar.Cache);

                registration.RegisterAll(brokerFactory);

                var factory = new SnapshotFactory(brokerFactory, registration.Cache);

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
                var comm = new BrokerConnectionClient();
                var client = comm.Create(config.Broker);

                var brokerObjectRegistrar = new BrokerObjectRegistration();
                brokerObjectRegistrar.RegisterAll(client);

                var registration = new SnapshotRegistration();
                var brokerFactory = new BrokerObjectFactory(client, brokerObjectRegistrar.Cache);

                registration.RegisterAll(brokerFactory);

                var factory = new SnapshotFactory(brokerFactory, registration);

                var resource = factory
                    .Snapshot<BrokerQueues>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .Execute();
            }
        }

        [Test]
        public void Test3()
        {
            var configurationProvider = new ConfigurationProvider();
            
            if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
            {
                var comm = new BrokerConnectionClient();
                var factory = new SnapshotFactory(new BrokerObjectFactory(comm.Create(config.Broker)));

                var resource = factory
                    .Snapshot<BrokerQueues>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .Execute();
            }
        }

        [Test]
        public void Test4()
        {
            var configurationProvider = new ConfigurationProvider();
            
            if (configurationProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/config.yaml", out HareDuConfig config))
            {
                var comm = new BrokerConnectionClient();
                var factory = new SnapshotFactory(comm.Create(config.Broker));

                var resource = factory
                    .Snapshot<BrokerQueues>()
                    .RegisterObserver(new BrokerQueuesJsonExporter())
                    .Execute();
            }
        }

        [Test]
        public void Test5()
        {
            var factory = new SnapshotFactory(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.UsingCredentials("guest", "guest");
            });

            var resource = factory
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new BrokerQueuesJsonExporter())
                .Execute();
        }

        [Test]
        public void Test6()
        {
            var factory = new SnapshotFactory();

            var resource = factory
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new BrokerQueuesJsonExporter())
                .Execute();
        }
    }
}