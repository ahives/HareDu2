﻿// Copyright 2013-2019 Albert L. Hives
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
    public class QueueTests
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
        public async Task Verify_can_create_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue31");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg => { arg.SetQueueExpiration(1000); });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Should_be_able_to_get_all_queues()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll();
            
            foreach (var queue in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_get_all_json()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("TestQueue10");
                    x.Target(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
//                        c.HasNoConsumers();
//                        c.IsEmpty();
                    });
                });

//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_peek_messages()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("Queue1");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_empty_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}