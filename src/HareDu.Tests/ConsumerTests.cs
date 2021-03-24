namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using Core.Extensions;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ConsumerTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_all_consumers()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Consumer>()
                .GetAll()
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2);
            result.Data[0].ChannelDetails.ShouldNotBeNull();
            result.Data[0].ChannelDetails?.Name.ShouldBe("127.0.0.0:61113 -> 127.0.0.0:5672 (1)");
            result.Data[0].ChannelDetails?.Node.ShouldBe("rabbit@localhost");
            result.Data[0].ChannelDetails?.Number.ShouldBe(1);
            result.Data[0].ChannelDetails?.ConnectionName.ShouldBe("127.0.0.0:61113 -> 127.0.0.0:5672");
            result.Data[0].ChannelDetails?.PeerHost.ShouldBe("127.0.0.0");
            result.Data[0].ChannelDetails?.PeerPort.ShouldBe(99883);
            result.Data[0].ChannelDetails?.User.ShouldBe("guest");
            result.Data[0].QueueConsumerDetails.ShouldNotBeNull();
            result.Data[0].QueueConsumerDetails?.Name.ShouldBe("fake_queue");
            result.Data[0].QueueConsumerDetails?.VirtualHost.ShouldBe("TestVirtualHost");
            result.Data[0].ConsumerTag.ShouldBe("amq.ctag-fOtZo9ajuHDYQQ5hzrgasA");
            result.Data[0].PreFetchCount.ShouldBe<ulong>(0);
            result.Data[0].Exclusive.ShouldBeFalse();
            result.Data[0].AcknowledgementRequired.ShouldBeTrue();
        }
        
        [Test]
        public async Task Verify_can_get_all_consumers1()
        {
            var services = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Consumer>()
                .GetAll();

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data[0].ChannelDetails);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672 (1)", result.Data[0].ChannelDetails?.Name);
                Assert.AreEqual("rabbit@localhost", result.Data[0].ChannelDetails?.Node);
                Assert.AreEqual(1, result.Data[0].ChannelDetails?.Number);
                Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672", result.Data[0].ChannelDetails?.ConnectionName);
                Assert.AreEqual("127.0.0.0", result.Data[0].ChannelDetails?.PeerHost);
                Assert.AreEqual(99883, result.Data[0].ChannelDetails?.PeerPort);
                Assert.AreEqual("guest", result.Data[0].ChannelDetails?.User);
                Assert.IsNotNull(result.Data[0].QueueConsumerDetails);
                Assert.AreEqual("fake_queue", result.Data[0].QueueConsumerDetails?.Name);
                Assert.AreEqual("TestVirtualHost", result.Data[0].QueueConsumerDetails?.VirtualHost);
                Assert.AreEqual("amq.ctag-fOtZo9ajuHDYQQ5hzrgasA", result.Data[0].ConsumerTag);
                Assert.AreEqual(0, result.Data[0].PreFetchCount);
                Assert.IsFalse(result.Data[0].Exclusive);
                Assert.IsTrue(result.Data[0].AcknowledgementRequired);
            });
        }

        [Test]
        public async Task Verify_can_get_all_consumers2()
        {
            var services = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllConsumers();

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data[0].ChannelDetails);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672 (1)", result.Data[0].ChannelDetails?.Name);
                Assert.AreEqual("rabbit@localhost", result.Data[0].ChannelDetails?.Node);
                Assert.AreEqual(1, result.Data[0].ChannelDetails?.Number);
                Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672", result.Data[0].ChannelDetails?.ConnectionName);
                Assert.AreEqual("127.0.0.0", result.Data[0].ChannelDetails?.PeerHost);
                Assert.AreEqual(99883, result.Data[0].ChannelDetails?.PeerPort);
                Assert.AreEqual("guest", result.Data[0].ChannelDetails?.User);
                Assert.IsNotNull(result.Data[0].QueueConsumerDetails);
                Assert.AreEqual("fake_queue", result.Data[0].QueueConsumerDetails?.Name);
                Assert.AreEqual("TestVirtualHost", result.Data[0].QueueConsumerDetails?.VirtualHost);
                Assert.AreEqual("amq.ctag-fOtZo9ajuHDYQQ5hzrgasA", result.Data[0].ConsumerTag);
                Assert.AreEqual(0, result.Data[0].PreFetchCount);
                Assert.IsFalse(result.Data[0].Exclusive);
                Assert.IsTrue(result.Data[0].AcknowledgementRequired);
            });
        }
    }
}