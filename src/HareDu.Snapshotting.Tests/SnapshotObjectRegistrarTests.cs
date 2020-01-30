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
    using System;
    using Autofac;
    using Core;
    using Core.Configuration;
    using Core.Extensions;
    using Fakes;
    using HareDu.Registration;
    using Moq;
    using Moq.Protected;
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

            builder.RegisterType<SnapshotInstanceCreator>()
                .As<ISnapshotInstanceCreator>()
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
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_1()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();

            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_2()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();

            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_with_others_3()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(4);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();

            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            
            registrar.ObjectCache.Count.ShouldBe(5);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerConnectivityImpl","HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.BrokerQueuesImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Internal.ClusterNodeImpl", "HareDu.Snapshotting")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_1()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_2()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.TryRegister(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_3()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_register_parameterless_snapshot_implementation_without_others_4()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);

            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_1()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);
            
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_2()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);
            
            registrar.Register<FakeHareDuSnapshotImpl>();
            registrar.Register<FakeHareDuSnapshotImpl>();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_3()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);
            
            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeTrue();
            registrar.TryRegister<FakeHareDuSnapshotImpl>().ShouldBeFalse();
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_identical_parameterless_snapshot_implementations_4()
        {
            var creator = _container.Resolve<ISnapshotInstanceCreator>();
            var registrar = new SnapshotObjectRegistrar(creator);
            
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            registrar.Register(typeof(FakeHareDuSnapshotImpl));
            
            registrar.ObjectCache.Count.ShouldBe(1);
            registrar.ObjectCache[GetKey("HareDu.Snapshotting.Tests.Fakes.FakeHareDuSnapshotImpl", "HareDu.Snapshotting.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_not_register_any_objects_if_instance_null()
        {
            var mock = new Mock<ISnapshotInstanceCreator>();

            mock
                .Setup(x => x.CreateInstance(It.IsAny<Type>()))
                .Returns(null)
                .Verifiable();

            var registrar = new SnapshotObjectRegistrar(mock.Object);
            
            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(0);
            mock.VerifyAll();
        }

        [Test]
        public void Verify_will_not_register_any_objects_if_instance_throws()
        {
            var mock = new Mock<ISnapshotInstanceCreator>();

            mock
                .Setup(x => x.CreateInstance(It.IsAny<Type>()))
                .Throws<Exception>()
                .Verifiable();

            var registrar = new SnapshotObjectRegistrar(mock.Object);
            
            registrar.RegisterAll();
            
            registrar.ObjectCache.Count.ShouldBe(0);
            mock.VerifyAll();
        }

        string GetKey(string className, string assembly)
        {
            Type type = Type.GetType($"{className}, {assembly}");

            return type.GetIdentifier();
        }
    }
}