namespace HareDu.Tests
{
    using System;
    using Core.Extensions;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;
    using Shouldly;

    [TestFixture]
    public class QueueTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_queues()
        {
            var container = GetContainerBuilder("TestData/QueueInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data[5].MessageStats.ShouldNotBeNull();
            result.Data[5].MessageStats.TotalMessageGets.ShouldBe<ulong>(0);
            result.Data[5].MessageStats.MessageGetDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessageGetDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessagesAcknowledged.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessagesAcknowledgedDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessagesDelivered.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessageDeliveryDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessagesPublished.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessagesPublishedDetails.Value.ShouldBe(1000.0M);
            result.Data[5].MessageStats.TotalMessagesRedelivered.ShouldBe<ulong>(0);
            result.Data[5].MessageStats.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessagesRedeliveredDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessageDeliveryGets.ShouldBe<ulong>(50000);
            result.Data[5].MessageStats.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessageDeliveryGetDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data[5].MessageStats.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessagesDeliveredWithoutAckDetails.Value.ShouldBe(0.0M);
            result.Data[5].MessageStats.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data[5].MessageStats.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data[5].MessageStats.MessageGetsWithoutAckDetails.Value.ShouldBe(0.0M);
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
            result.Data[5].ReductionDetails?.Value.ShouldBe(0.0M);
            result.Data[5].UnacknowledgedMessageDetails?.Value.ShouldBe(0.0M);
            result.Data[5].ReadyMessageDetails?.Value.ShouldBe(0.0M);
            result.Data[5].MessageDetails?.Value.ShouldBe(0.0M);
            result.Data[5].Name.ShouldBe("consumer_queue");
            result.Data[5].Node.ShouldBe("rabbit@localhost");
            result.Data[5].IdleSince.ShouldBe(DateTimeOffset.Parse("2019-11-09 11:57:45"));
            result.Data[5].State.ShouldBe(QueueState.Running);
            result.Data[5].VirtualHost.ShouldBe("HareDu");
        }

        [Test]
        public void Verify_can_create_queue()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
//            definition.Arguments["x-expires"].ShouldBe(1000);
//            definition.Arguments["x-message-ttl"].ShouldBe(2000);
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_cannot_create_queue_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("1000");
            definition.Arguments["x-message-ttl"].ToString().ShouldBe("2000");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_can_override_arguments()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            
            definition.Arguments["x-expires"].ToString().ShouldBe("980");
            definition.Durable.ShouldBeTrue();
            definition.AutoDelete.ShouldBeTrue();
            definition.Node.ShouldBe("Node1");
        }

        [Test]
        public void Verify_can_delete_queue()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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