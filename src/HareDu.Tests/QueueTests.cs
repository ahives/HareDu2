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
namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_queues()
        {
            var container = GetContainerBuilder("TestData/QueueInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
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
        public async Task Verify_can_peek_messages()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("Queue1");
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });

//            foreach (var message in result.Select(x => x.Data))
//            {
//                Console.WriteLine(message.PayloadBytes);
//                Console.WriteLine(message.MessageCount);
//                Console.WriteLine(message.Properties);
//                Console.WriteLine(message.Payload);
//            }
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_create_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue31");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.AutoDeleteWhenNotInUse();
                        c.HasArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                            arg.SetPerQueuedMessageExpiration(2000);
                        });
                    });
                    x.Target(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                });
            
            Assert.IsFalse(result.HasFaulted);

            QueueDefinition request = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            Assert.IsTrue(request.AutoDelete);
            Assert.AreEqual("Node1", request.Node);
            Assert.IsTrue(request.Durable);
            Assert.AreEqual(1000, request.Arguments["x-expires"]);
            Assert.AreEqual(2000,request.Arguments["x-message-ttl"]);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("TestQueue10");
                    x.Target(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
//                        c.IsEmpty();
                    });
                });
            
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("api/queues/HareDu/TestQueue10?if-unused=true", result.DebugInfo.URL);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_empty_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("Node1");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("api/queues/HareDu/Node1/contents", result.DebugInfo.URL);
            Console.WriteLine(result.ToJsonString());
        }
    }
}