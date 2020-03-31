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
namespace HareDu.Tests.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class QueueTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_queues()
        {
            var container = GetContainerBuilder("TestData/QueueInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data[5].MessageStats.ShouldNotBeNull();
            result.Data[5].MessageStats?.TotalMessageGets.ShouldBe<ulong>(0);
            result.Data[5].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats?.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessagesAcknowledgedDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats?.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessageDeliveryDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats?.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessagesPublishedDetails?.Rate.ShouldBe(1000.0M);
            result.Data[5].MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(0);
            result.Data[5].MessageStats?.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats?.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessageDeliveryGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data[5].MessageStats?.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data[5].MessageStats?.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data[5].MessageStats?.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].Consumers.ShouldBe<ulong>(1);
            result.Data[5].Durable.ShouldBeTrue();
            result.Data[5].Exclusive.ShouldBeFalse();
            result.Data[5].AutoDelete.ShouldBeFalse();
            result.Data[5].Memory.ShouldBe<ulong>(17628);
            result.Data[5].MessageBytesPersisted.ShouldBe<ulong>(0);
            result.Data[5].MessageBytesInRAM.ShouldBe<ulong>(100);
            result.Data[5].MessageBytesPagedOut.ShouldBe<ulong>(10);
            result.Data[5].TotalBytesOfAllMessages.ShouldBe<ulong>(10000);
            result.Data[5].UnacknowledgedMessages.ShouldBe<ulong>(30);
            result.Data[5].ReadyMessages.ShouldBe<ulong>(50);
            result.Data[5].MessagesInRAM.ShouldBe<ulong>(50);
            result.Data[5].TotalMessages.ShouldBe<ulong>(6700);
            result.Data[5].UnacknowledgedMessagesInRAM.ShouldBe<ulong>(30000);
            result.Data[5].TotalReductions.ShouldBe(77349645);
            result.Data[5].ReductionDetails.ShouldNotBeNull();
            result.Data[5].ReductionDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].UnacknowledgedMessageDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].ReadyMessageDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].MessageDetails?.Rate.ShouldBe(0.0M);
            result.Data[5].Name.ShouldBe("consumer_queue");
            result.Data[5].Node.ShouldBe("rabbit@localhost");
            result.Data[5].IdleSince.ShouldBe(DateTimeOffset.Parse("2019-11-09 11:57:45"));
            result.Data[5].State.ShouldBe("running");
            result.Data[5].VirtualHost.ShouldBe("HareDu");
        }

        [Test]
        public void Verify_can_peek_messages()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data[0].ShouldNotBeNull();
            result.Data[0]?.Exchange.ShouldBe("HareDu.IntegrationTesting.Core:FakeMessage");
            result.Data[0]?.Payload.ShouldNotBeNull();
            result.Data[0]?.Payload["messageId"].Cast<string>().ShouldBe("b64a0000-0481-dca9-a948-08d7650c25d3");
            result.Data[0]?.Payload["conversationId"].Cast<string>().ShouldBe("b64a0000-0481-dca9-aac4-08d7650c25d3");
            result.Data[0]?.Properties.ShouldNotBeNull();
            result.Data[0]?.Properties?.ContentType.ShouldNotBeNull();
            result.Data[0]?.Properties?.ContentType.ShouldBe("application/vnd.masstransit+json");
            result.Data[0]?.Properties?.CorrelationId.ShouldBe("b64a0000-0481-dca9-8c2c-08d7650c1eeb");
            result.Data[0]?.Properties?.MessageId.ShouldBe("b64a0000-0481-dca9-a948-08d7650c25d3");
            result.Data[0]?.Properties?.DeliveryMode.ShouldBe<uint>(2);
            result.Data[0]?.MessageCount.ShouldBe<ulong>(49999);
            result.Data[0]?.RoutingKey.ShouldBeNullOrEmpty();
            result.Data[0]?.Redelivered.ShouldBeTrue();
            result.Data[0]?.Properties?.Headers.ShouldNotBeNull();
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);

            result.Data[0]?.Properties?.Headers["Content-Type"].ShouldBe("application/vnd.masstransit+json");
            result.Data[0]?.Properties?.Headers["publishId"].ShouldBe("1");
        }

        [Test]
        public void Verify_cannot_peek_messages_1()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue(string.Empty);
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_2()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_3()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_4()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => {});
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_5()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_6()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue(string.Empty);
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_7()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => {});
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_8()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.Take(1);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(1);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_9()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("Queue1");
                    x.Configure(c =>
                    {
                        c.Take(0);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_10()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.Take(0);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_11()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue(string.Empty);
                    x.Configure(c =>
                    {
                        c.Take(0);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_12()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_13()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Configure(c =>
                    {
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(3);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBe("auto");
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

        [Test]
        public void Verify_cannot_peek_messages_14()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(4);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBeNullOrEmpty();
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBeNullOrEmpty();
            definition.TruncateMessageThreshold.ShouldBe<ulong>(0);
        }

        [Test]
        public void Verify_cannot_peek_messages_15()
        {
            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("Queue1");
                    x.Configure(c =>
                    {
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.HasData.ShouldBeFalse();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(3);
            result.DebugInfo.ShouldNotBeNull();
            
            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
            
            definition.Encoding.ShouldBeNullOrEmpty();
            definition.Take.ShouldBe<uint>(0);
            definition.RequeueMode.ShouldBe("ack_requeue_true");
            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
        }

//        [Test]
//        public async Task Verify_cannot_peek_messages_()
//        {
//            var container = GetContainerBuilder("TestData/PeekedMessageInfo.json").Build();
//            var result = await container.Resolve<IBrokerObjectFactory>()
//                .Object<Queue>()
//                .Peek(x =>
//                {
//                    x.Queue("Queue1");
//                    x.Configure(c =>
//                    {
//                        c.Take(1);
//                        c.AckMode(RequeueMode.AckRequeue);
//                        c.TruncateIfAbove(5000);
//                        c.Encoding(MessageEncoding.Auto);
//                    });
//                    x.Target(t => t.VirtualHost("HareDu"));
//                });
//
//            result.HasFaulted.ShouldBeTrue();
//            result.HasData.ShouldBeFalse();
//            result.Data.ShouldBeNull();
//            result.Errors.Count.ShouldBe(1);
//            
//            QueuePeekDefinition definition = result.DebugInfo.Request.ToObject<QueuePeekDefinition>();
//            
//            definition.Encoding.ShouldBe("auto");
//            definition.Take.ShouldBe<uint>(1);
//            definition.RequeueMode.ShouldBe("ack_requeue_true");
//            definition.TruncateMessageThreshold.ShouldBe<ulong>(5000);
//        }

        [Test]
        public void Verify_can_create_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
//            definition.Arguments["x-expires"].ShouldBe(1000);
//            definition.Arguments["x-message-ttl"].ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue(string.Empty);
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
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
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
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t =>
                    {
                        t.VirtualHost(string.Empty);
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
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
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
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
                    x.Targeting(t =>
                    {
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
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
                    x.Targeting(t =>
                    {
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(1000);
            definition.Arguments["x-message-ttl"].Cast<long>().ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_can_override_arguments()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                            arg.Set<long>("x-expires", 980);
                        });
                    });
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                        t.Node("Node1");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            QueueDefinition definition = result.DebugInfo.Request.ToObject<QueueDefinition>();
            
            definition.Arguments["x-expires"].Cast<long>().ShouldBe(980);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_can_delete_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            result.DebugInfo.URL.ShouldBe("api/queues/HareDu/Queue1?if-unused=true");
        }

        [Test]
        public void Verify_cannot_delete_queue_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue(string.Empty);
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_queue_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_queue_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(l => l.VirtualHost(string.Empty));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_queue_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(l => {});
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_queue_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("Queue1");
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_queue_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue(string.Empty);
                    x.Targeting(l => l.VirtualHost(string.Empty));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_queue_()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_can_empty_queue()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_empty_queue_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue(string.Empty);
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("Queue1");
                    x.Targeting(t => {});
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("Queue1");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_empty_queue_7()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue(string.Empty);
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_empty_queue_8()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}