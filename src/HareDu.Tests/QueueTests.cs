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

            Assert.IsFalse(result.HasFaulted);
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);
//            Assert.AreEqual(, result.Data[5].MessageStats);
            Assert.IsNotNull(result.Data[5].MessageStats);
            Assert.AreEqual(0, result.Data[5].MessageStats?.TotalMessageGets);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessageGetDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessageGetDetails?.Rate);
            Assert.AreEqual(50000, result.Data[5].MessageStats?.TotalMessagesAcknowledged);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessagesAcknowledgedDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessagesAcknowledgedDetails?.Rate);
            Assert.AreEqual(50000, result.Data[5].MessageStats?.TotalMessagesDelivered);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessageDeliveryDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessageDeliveryDetails?.Rate);
            Assert.AreEqual(50000, result.Data[5].MessageStats?.TotalMessagesPublished);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessagesPublishedDetails);
            Assert.AreEqual(1000.0, result.Data[5].MessageStats?.MessagesPublishedDetails?.Rate);
            Assert.AreEqual(0, result.Data[5].MessageStats?.TotalMessagesRedelivered);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessagesRedeliveredDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessagesRedeliveredDetails?.Rate);
            Assert.AreEqual(50000, result.Data[5].MessageStats?.TotalMessageDeliveryGets);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessageDeliveryGetDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessageDeliveryGetDetails?.Rate);
            Assert.AreEqual(0, result.Data[5].MessageStats?.TotalMessageDeliveredWithoutAck);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessagesDeliveredWithoutAckDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate);
            Assert.AreEqual(0, result.Data[5].MessageStats?.TotalMessageGetsWithoutAck);
            Assert.IsNotNull(result.Data[5].MessageStats?.MessageGetsWithoutAckDetails);
            Assert.AreEqual(0.0, result.Data[5].MessageStats?.MessageGetsWithoutAckDetails?.Rate);
            Assert.AreEqual(1, result.Data[5].Consumers);
            Assert.IsTrue(result.Data[5].Durable);
            Assert.IsFalse(result.Data[5].Exclusive);
            Assert.IsFalse(result.Data[5].AutoDelete);
            Assert.AreEqual(17628, result.Data[5].Memory);
            Assert.AreEqual(0, result.Data[5].MessageBytesPersisted);
            Assert.AreEqual(100, result.Data[5].MessageBytesInRam);
            Assert.AreEqual(10, result.Data[5].MessageBytesPagedOut);
            Assert.AreEqual(10000, result.Data[5].TotalBytesOfAllMessages);
            Assert.AreEqual(30, result.Data[5].UnacknowledgedMessages);
            Assert.AreEqual(50, result.Data[5].ReadyMessages);
            Assert.AreEqual(50, result.Data[5].MessagesInRam);
            Assert.AreEqual(6700, result.Data[5].TotalMessages);
            Assert.AreEqual(30000, result.Data[5].UnacknowledgedMessagesInRam);
            Assert.AreEqual(77349645, result.Data[5].TotalReductions);
            Assert.IsNotNull(result.Data[5].ReductionRate);
            Assert.AreEqual(0.0, result.Data[5].ReductionRate?.Rate);
            Assert.AreEqual(0.0, result.Data[5].UnackedMessageRate?.Rate);
            Assert.AreEqual(0.0, result.Data[5].ReadyMessageRate?.Rate);
            Assert.AreEqual(0.0, result.Data[5].MessageRate?.Rate);
//            Assert.AreEqual(, result.Data[5]);
//            Assert.AreEqual(, result.Data[5]);
//            Assert.AreEqual(, result.Data[5]);
            Assert.AreEqual("consumer_queue", result.Data[5].Name);
            Assert.AreEqual("rabbit@localhost", result.Data[5].Node);
            Assert.AreEqual(DateTimeOffset.Parse("2019-11-09 11:57:45"), result.Data[5].IdleSince);
            Assert.AreEqual("running", result.Data[5].State);
            Assert.AreEqual("HareDu", result.Data[5].VirtualHost);
//            Assert.AreEqual("", result.Data[5]);
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