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
    using Core;
    using Core.Configuration;
    using Core.Extensions;
    using HareDu.Registration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BrokerObjectRegistrarTests
    {
        [Test]
        public void Verify_will_register_all_broker_objects_1()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);

            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(comm.GetClient(settings));
            var registrar = new BrokerObjectRegistrar(finder, creator);
            
            registrar.RegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_2()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);

            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(comm.GetClient(settings));
            var registrar = new BrokerObjectRegistrar(finder, creator);

            registrar.TryRegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_1()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            var client = comm.GetClient(settings);

            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(comm.GetClient(settings));
            var registrar = new BrokerObjectRegistrar(finder, creator);

            registrar.RegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();

            registrar.Register(typeof(TestImpl), client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Tests.Registration.TestImpl", "HareDu.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_2()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            
            var client = comm.GetClient(settings);
            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(client);
            var registrar = new BrokerObjectRegistrar(finder, creator);

            registrar.RegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();

            registrar.TryRegister(typeof(TestImpl), client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Tests.Registration.TestImpl", "HareDu.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_3()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            
            var client = comm.GetClient(settings);
            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(client);
            var registrar = new BrokerObjectRegistrar(finder, creator);
            
            registrar.RegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();

            registrar.Register<TestImpl>(client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Tests.Registration.TestImpl", "HareDu.Tests")].ShouldNotBeNull();
        }

        [Test]
        public void Verify_will_register_all_broker_objects_plus_one_4()
        {
            var comm = new BrokerCommunication();
            var configProvider = new BrokerConfigProvider(new ConfigurationProvider());

            configProvider.TryGet(out BrokerConfig settings);
            
            var client = comm.GetClient(settings);
            var finder = new BrokerObjectTypeFinder();
            var creator = new BrokerObjectInstanceCreator(client);
            var registrar = new BrokerObjectRegistrar(finder, creator);
            
            registrar.RegisterAll();

            registrar.ObjectCache.Count.ShouldBe(20);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();

            registrar.TryRegister<TestImpl>(client);

            registrar.ObjectCache.Count.ShouldBe(21);
            registrar.ObjectCache[GetKey("HareDu.Internal.BindingImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ChannelImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConnectionImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ConsumerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ExchangeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.GlobalParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.MemoryImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.NodeImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.PolicyImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.QueueImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ScopedParameterImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerHealthImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.ServerImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.SystemOverviewImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.TopicPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.UserPermissionsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Internal.VirtualHostLimitsImpl", "HareDu")].ShouldNotBeNull();
            registrar.ObjectCache[GetKey("HareDu.Tests.Registration.TestImpl", "HareDu.Tests")].ShouldNotBeNull();
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