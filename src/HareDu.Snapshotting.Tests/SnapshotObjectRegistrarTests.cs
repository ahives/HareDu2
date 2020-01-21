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
namespace HareDu.Snapshotting.Tests
{
    using Autofac;
    using Core;
    using Core.Configuration;
    using Fakes;
    using HareDu.Registration;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class SnapshotObjectRegistrarTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            builder.Register(x =>
                {
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var comm = x.Resolve<IBrokerCommunication>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    return new BrokerObjectFactory(comm.GetClient(settings), x.Resolve<IBrokerObjectRegistrar>());
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();
            
            builder.Register(x =>
                {
                    var comm = x.Resolve<IBrokerCommunication>();
                    var configProvider = x.Resolve<IBrokerConfigProvider>();

                    if (!configProvider.TryGet(out var config))
                        throw new HareDuClientConfigurationException(
                            "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    var registrar = new BrokerObjectRegistrar();

                    registrar.RegisterAll(comm.GetClient(config));

                    return registrar;
                })
                .As<IBrokerObjectRegistrar>()
                .SingleInstance();

            builder.RegisterType<BrokerCommunication>()
                .As<IBrokerCommunication>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_can_register_all_concrete_classes_that_implement_HareDuSnapshot_interface()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_1()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();

            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_2()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();

            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_3()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();

            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerConnectivityImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.BrokerQueuesImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Internal.ClusterNodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_1()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_2()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.TryRegister(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_3()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_4()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);

            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_1()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);
            
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_2()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);
            
            registrar.Register<FakeHareDuSnapshotImpl>();
            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_3()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);
            
            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeFalse();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_4()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();
            var registrar = new SnapshotObjectRegistrar(factory);
            
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache["HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl"].ShouldNotBeNull();
        }
    }
}