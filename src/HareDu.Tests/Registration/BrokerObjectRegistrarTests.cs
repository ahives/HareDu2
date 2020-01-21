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
namespace HareDu.Tests.Registration
{
    using System.Net.Http;
    using System.Reflection;
    using Core;
    using Core.Configuration;
    using HareDu.Registration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BrokerObjectRegistrarTests
    {
        [Test]
        public void Verify_will_register_all_broker_objects_1()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            
            registrar.RegisterAll(comm.GetClient(settings));

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_2()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            
            registrar.TryRegisterAll(comm.GetClient(settings));

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_all_broker_objects_1()
        {
            var registrar = new BrokerObjectRegistrar();
            
            registrar.RegisterAll(null);

            registrar.ObjectCache.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_cannot_register_all_broker_objects_2()
        {
            var registrar = new BrokerObjectRegistrar();
            
            registrar.TryRegisterAll(null);

            registrar.ObjectCache.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_1()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            
            registrar.RegisterAll(client);

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();

            registrar.Register(typeof(TestImpl), client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_2()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            
            registrar.RegisterAll(client);

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();

            registrar.TryRegister(typeof(TestImpl), client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_3()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            
            registrar.RegisterAll(client);

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            
            registrar.Register<TestImpl>(client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_4()
        {
            var registrar = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            
            registrar.RegisterAll(client);

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            
            registrar.TryRegister<TestImpl>(client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registrar.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_cannot_register_broker_object_1()
        {
            var registrar = new BrokerObjectRegistrar();
            
            registrar.Register<TestImpl>(null);

            registrar.ObjectCache.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_cannot_register_broker_object_2()
        {
            var registrar = new BrokerObjectRegistrar();

            registrar.Register(typeof(TestImpl), null);

            registrar.ObjectCache.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_cannot_register_broker_object_3()
        {
            var registrar = new BrokerObjectRegistrar();
            
            registrar.TryRegister<TestImpl>(null);

            registrar.ObjectCache.Count.ShouldBe(0);
        }
    }

    class TestImpl :
        BaseBrokerObject,
        TestBrokerObject
    {
        public TestImpl(HttpClient client)
            : base(client)
        {
        }
    }

    interface TestBrokerObject :
        BrokerObject
    {
    }
}