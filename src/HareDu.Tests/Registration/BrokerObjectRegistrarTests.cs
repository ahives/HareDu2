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
        public void Verify_will_throw_if_param_not_present()
        {
            var registry = new BrokerObjectRegistrar();

            Should.Throw<TargetInvocationException>(() => registry.RegisterAll(null));
        }

        [Test]
        public void Verify_will_register_all_broker_objects()
        {
            var registry = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            registry.RegisterAll(comm.GetClient(settings));

            registry.ObjectCache.Count.ShouldBe(20);
            registry.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_1()
        {
            var registry = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            registry.RegisterAll(client);
            registry.Register(client, typeof(TestImpl));

            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_2()
        {
            var registry = new BrokerObjectRegistrar();
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);
            registry.RegisterAll(client);
            registry.Register<TestImpl>(client);

            registry.ObjectCache.Count.ShouldBe(21);
            registry.ObjectCache["HareDu.Internal.BindingImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ChannelImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConnectionImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ConsumerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ExchangeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.GlobalParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.MemoryImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.NodeImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.PolicyImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.QueueImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ScopedParameterImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerHealthImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.ServerImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.SystemOverviewImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.TopicPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.UserPermissionsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Internal.VirtualHostLimitsImpl"].ShouldNotBeNull();
            registry.ObjectCache["HareDu.Tests.Registration.TestImpl"].ShouldNotBeNull();
        }
    }

    class TestImpl :
        TestBrokerObject
    {
        public TestImpl(HttpClient client)
        {
        }
    }

    interface TestBrokerObject :
        BrokerObject
    {
    }
}