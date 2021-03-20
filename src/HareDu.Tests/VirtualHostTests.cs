namespace HareDu.Tests
{
    using Core.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_vhosts()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .GetResult();
            
            result.HasData.ShouldBeTrue();
            result.Data.Count.ShouldBe(3);
            result.HasFaulted.ShouldBeFalse();
            result.Data[2].Name.ShouldBe("TestVirtualHost");
            result.Data[2].PacketBytesReceived.ShouldBe<ulong>(301363575);
            result.Data[2].PacketBytesReceivedDetails.ShouldNotBeNull();
            result.Data[2].PacketBytesReceivedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].PacketBytesSent.ShouldBe<ulong>(368933935);
            result.Data[2].PacketBytesSentDetails.ShouldNotBeNull();
            result.Data[2].PacketBytesSentDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].TotalMessages.ShouldBe<ulong>(0);
            result.Data[2].MessagesDetails.ShouldNotBeNull();
            result.Data[2].MessagesDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].ReadyMessages.ShouldBe<ulong>(0);
            result.Data[2].ReadyMessagesDetails.ShouldNotBeNull();
            result.Data[2].ReadyMessagesDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats.ShouldNotBeNull();
            result.Data[2].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesConfirmedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.UnroutableMessagesDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.TotalMessagesConfirmed.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesConfirmedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesPublishedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalUnroutableMessages.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.UnroutableMessagesDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesAcknowledgedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessageDeliveryDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(300003);
            result.Data[2].MessageStats?.MessageDeliveryGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
        }

        [Test]
        public void Verify_Create_works()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu7");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                })
                .GetResult();
            
            result.DebugInfo.ShouldNotBeNull();
            
            VirtualHostDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostDefinition>();

            definition.Tracing.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_delete()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"))
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost(string.Empty))
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_2()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => {})
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_can_start_vhost()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("FakeVirtualHost", x => x.On("FakeNode"))
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_start_vhost_1()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => x.On("FakeNode"))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_start_vhost_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("FakeVirtualHost", x => x.On(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_start_vhost_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => x.On(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_start_vhost_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => {})
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}