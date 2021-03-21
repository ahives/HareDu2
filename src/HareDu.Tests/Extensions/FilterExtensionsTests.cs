namespace HareDu.Tests.Extensions
{
    using Core.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class FilterExtensionsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_Where_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").BuildServiceProvider();
            var vhosts = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "TestVirtualHost");

            vhosts.Count.ShouldBe(1);
            vhosts[0].Name.ShouldBe("TestVirtualHost");
            vhosts[0].PacketBytesReceived.ShouldBe<ulong>(301363575);
            vhosts[0].PacketBytesReceivedDetails.ShouldNotBeNull();
            vhosts[0].PacketBytesReceivedDetails?.Value.ShouldBe(0.0M);
            vhosts[0].PacketBytesSent.ShouldBe<ulong>(368933935);
            vhosts[0].PacketBytesSentDetails.ShouldNotBeNull();
            vhosts[0].PacketBytesSentDetails?.Value.ShouldBe(0.0M);
            vhosts[0].TotalMessages.ShouldBe<ulong>(0);
            vhosts[0].MessagesDetails.ShouldNotBeNull();
            vhosts[0].MessagesDetails?.Value.ShouldBe(0.0M);
            vhosts[0].ReadyMessages.ShouldBe<ulong>(0);
            vhosts[0].ReadyMessagesDetails.ShouldNotBeNull();
            vhosts[0].ReadyMessagesDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.ShouldNotBeNull();
            vhosts[0].MessageStats.TotalMessageGets.ShouldBe<ulong>(3);
            vhosts[0].MessageStats.MessageGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessageGetDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.MessagesConfirmedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessagesPublishedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.UnroutableMessagesDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessagesAcknowledgedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessageDeliveryDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessageDeliveryGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessageGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.MessagesRedeliveredDetails.ShouldNotBeNull();
            vhosts[0].MessageStats.TotalMessagesConfirmed.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats.MessagesConfirmedDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessagesPublished.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats.MessagesPublishedDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalUnroutableMessages.ShouldBe<ulong>(0);
            vhosts[0].MessageStats.UnroutableMessagesDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessagesAcknowledged.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats.MessagesAcknowledgedDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessagesDelivered.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats.MessageDeliveryDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessageDeliveryGets.ShouldBe<ulong>(300003);
            vhosts[0].MessageStats.MessageDeliveryGetDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            vhosts[0].MessageStats.MessagesDeliveredWithoutAckDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessageGets.ShouldBe<ulong>(3);
            vhosts[0].MessageStats.MessageGetDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            vhosts[0].MessageStats.MessageGetsWithoutAckDetails?.Value.ShouldBe(0.0M);
            vhosts[0].MessageStats.TotalMessagesRedelivered.ShouldBe<ulong>(3);
            vhosts[0].MessageStats.MessagesRedeliveredDetails?.Value.ShouldBe(0.0M);
        }

        [Test]
        public void Verify_Select_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Select(x => x.Data);

            result.Count.ShouldBe(3);
            result[0].Name.ShouldBe("/");
            result[1].Name.ShouldBe("HareDu");
            result[2].Name.ShouldBe("TestVirtualHost");
        }
    }
}