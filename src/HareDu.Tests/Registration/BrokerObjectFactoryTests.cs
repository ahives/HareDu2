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
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Extensions;
    using HareDu.Registration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BrokerObjectFactoryTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu()
                .AddHareDuSnapshot()
                .Build();
        }

        [Test]
        public async Task Verify_can_return_Queue_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Queue>();
        }

        [Test]
        public async Task Verify_can_return_Exchange_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Exchange>();
        }

        [Test]
        public async Task Verify_can_return_Node_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Node>();
        }

        [Test]
        public async Task Verify_can_return_User_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<User>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<User>();
        }

        [Test]
        public async Task Verify_can_return_GlobalParameter_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<GlobalParameter>();
        }

        [Test]
        public async Task Verify_can_return_UserPermissions_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<UserPermissions>();
        }

        [Test]
        public async Task Verify_can_return_Policy_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Policy>();
        }

        [Test]
        public async Task Verify_can_return_Binding_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Binding>();
        }

        [Test]
        public async Task Verify_can_return_ScopedParameter_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<ScopedParameter>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<ScopedParameter>();
        }

        [Test]
        public async Task Verify_can_return_VirtualHostLimits_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<VirtualHostLimits>();
        }

        [Test]
        public async Task Verify_can_return_TopicPermissions_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<TopicPermissions>();
        }

        [Test]
        public async Task Verify_can_return_SystemOverview_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<SystemOverview>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<SystemOverview>();
        }

        [Test]
        public async Task Verify_can_return_Server_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Server>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Server>();
        }

        [Test]
        public async Task Verify_can_return_Consumer_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Consumer>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Consumer>();
        }

        [Test]
        public async Task Verify_can_return_Connection_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Connection>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Connection>();
        }

        [Test]
        public async Task Verify_can_return_Channel_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<Channel>();
        }

        [Test]
        public async Task Verify_can_return_VirtualHost_broker_object()
        {
            var impl = _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>();

            impl.ShouldNotBeNull();
            impl.ShouldBeAssignableTo<VirtualHost>();
        }

        [Test]
        public void Verify_all_broker_objects_are_registered()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();

            factory.IsRegistered("HareDu.Binding").ShouldBeTrue();
            factory.IsRegistered("HareDu.Channel").ShouldBeTrue();
            factory.IsRegistered("HareDu.Connection").ShouldBeTrue();
            factory.IsRegistered("HareDu.Consumer").ShouldBeTrue();
            factory.IsRegistered("HareDu.Exchange").ShouldBeTrue();
            factory.IsRegistered("HareDu.GlobalParameter").ShouldBeTrue();
            factory.IsRegistered("HareDu.Node").ShouldBeTrue();
            factory.IsRegistered("HareDu.Policy").ShouldBeTrue();
            factory.IsRegistered("HareDu.Queue").ShouldBeTrue();
            factory.IsRegistered("HareDu.ScopedParameter").ShouldBeTrue();
            factory.IsRegistered("HareDu.Server").ShouldBeTrue();
            factory.IsRegistered("HareDu.SystemOverview").ShouldBeTrue();
            factory.IsRegistered("HareDu.TopicPermissions").ShouldBeTrue();
            factory.IsRegistered("HareDu.User").ShouldBeTrue();
            factory.IsRegistered("HareDu.UserPermissions").ShouldBeTrue();
            factory.IsRegistered("HareDu.VirtualHost").ShouldBeTrue();
            factory.IsRegistered("HareDu.VirtualHostLimits").ShouldBeTrue();
        }

        [Test]
        public void Verify_can_register_new_broker_objects()
        {
            var factory = _container.Resolve<IBrokerObjectFactory>();

            factory.IsRegistered("HareDu.Binding").ShouldBeTrue();
            factory.IsRegistered("HareDu.Channel").ShouldBeTrue();
            factory.IsRegistered("HareDu.Connection").ShouldBeTrue();
            factory.IsRegistered("HareDu.Consumer").ShouldBeTrue();
            factory.IsRegistered("HareDu.Exchange").ShouldBeTrue();
            factory.IsRegistered("HareDu.GlobalParameter").ShouldBeTrue();
            factory.IsRegistered("HareDu.Node").ShouldBeTrue();
            factory.IsRegistered("HareDu.Policy").ShouldBeTrue();
            factory.IsRegistered("HareDu.Queue").ShouldBeTrue();
            factory.IsRegistered("HareDu.ScopedParameter").ShouldBeTrue();
            factory.IsRegistered("HareDu.Server").ShouldBeTrue();
            factory.IsRegistered("HareDu.SystemOverview").ShouldBeTrue();
            factory.IsRegistered("HareDu.TopicPermissions").ShouldBeTrue();
            factory.IsRegistered("HareDu.User").ShouldBeTrue();
            factory.IsRegistered("HareDu.UserPermissions").ShouldBeTrue();
            factory.IsRegistered("HareDu.VirtualHost").ShouldBeTrue();
            factory.IsRegistered("HareDu.VirtualHostLimits").ShouldBeTrue();

            factory.Object<TestBrokerObject>();
            factory.IsRegistered("HareDu.Tests.Registration.TestBrokerObject").ShouldBeTrue();
        }

        string GetKey(string className, string assembly)
        {
            Type type = Type.GetType($"{className}, {assembly}");

            return type.GetIdentifier();
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