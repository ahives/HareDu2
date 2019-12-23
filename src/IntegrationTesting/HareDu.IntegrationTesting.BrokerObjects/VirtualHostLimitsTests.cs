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
namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostLimitsTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_get_all_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll();
            
            foreach (var item in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", item.VirtualHostName);

                if (item.Limits.TryGetValue("max-connections", out ulong maxConnections))
                    Console.WriteLine("max-connections: {0}", maxConnections);

                if (item.Limits.TryGetValue("max-queues", out ulong maxQueues))
                    Console.WriteLine("max-queues: {0}", maxQueues);
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_get_limits_of_specified_vhost()
        {
            var result = _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll()
                .Where(x => x.VirtualHostName == "HareDu");

            foreach (var item in result)
            {
                Console.WriteLine("Name: {0}", item.VirtualHostName);

                if (item.Limits.TryGetValue("max-connections", out ulong maxConnections))
                    Console.WriteLine("max-connections: {0}", maxConnections);

                if (item.Limits.TryGetValue("max-queues", out ulong maxQueues))
                    Console.WriteLine("max-queues: {0}", maxQueues);
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_define_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("HareDu5");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                });
            
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("HareDu3"));
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}