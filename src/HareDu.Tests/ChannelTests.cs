namespace HareDu.Tests
{
    using Core.Extensions;
    using Internal.Model;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ChannelTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_all_channels()
        {
            var container = GetContainerBuilder("TestData/ChannelInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll()
                .GetResult();
            
            result.HasData.ShouldBeTrue();
            result.Data.Count.ShouldBe(2);
            result.HasFaulted.ShouldBeFalse();
            result.Data[0].Confirm.ShouldBeTrue();
            result.Data[0].UncommittedAcknowledgements.ShouldBe<ulong>(0);
            result.Data[0].GlobalPrefetchCount.ShouldBe<uint>(8);
            result.Data[0].UnacknowledgedMessages.ShouldBe<ulong>(0);
            result.Data[0].UncommittedMessages.ShouldBe<ulong>(0);
            result.Data[0].UnconfirmedMessages.ShouldBe<ulong>(0);
            result.Data[0].PrefetchCount.ShouldBe<uint>(0);
            result.Data[0].Number.ShouldBe<ulong>(1);
            result.Data[0].TotalReductions.ShouldBe<ulong>(6149);
            result.Data[0].ReductionDetails.ShouldNotBeNull();
            result.Data[0].ReductionDetails?.Value.ShouldBe(0.0M);
            result.Data[0].ConnectionDetails.ShouldNotBeNull();
            result.Data[0].ConnectionDetails?.Name.ShouldBe("127.0.0.0:72368 -> 127.0.0.0:5672");
            result.Data[0].ConnectionDetails?.PeerHost.ShouldBe("127.0.0.0");
            result.Data[0].ConnectionDetails?.PeerPort.ShouldBe(98343);
            result.Data[0].Transactional.ShouldBeFalse();
            result.Data[0].OperationStats.ShouldBeNull();
            result.Data[1].OperationStats.ShouldNotBeNull();
            result.Data[1].Confirm.ShouldBeTrue();
            result.Data[1].UncommittedAcknowledgements.ShouldBe<ulong>(0);
            result.Data[1].GlobalPrefetchCount.ShouldBe<uint>(64);
            result.Data[1].UnacknowledgedMessages.ShouldBe<ulong>(0);
            result.Data[1].UncommittedMessages.ShouldBe<ulong>(0);
            result.Data[1].UnconfirmedMessages.ShouldBe<ulong>(0);
            result.Data[1].PrefetchCount.ShouldBe<uint>(0);
            result.Data[1].Number.ShouldBe<ulong>(2);
            result.Data[1].TotalReductions.ShouldBe<ulong>(19755338);
            result.Data[1].Transactional.ShouldBeFalse();
            result.Data[1].ReductionDetails.ShouldNotBeNull();
            result.Data[1].ReductionDetails.Value.ShouldBe(0.0M);
            result.Data[1].Name.ShouldBe("127.0.0.0:72368 -> 127.0.0.0:5672 (2)");
            result.Data[1].Node.ShouldBe("rabbit@localhost");
            result.Data[1].User.ShouldBe("guest");
            result.Data[1].UserWhoPerformedAction.ShouldBe("guest");
            result.Data[1].VirtualHost.ShouldBe("TestVirtualHost");
            result.Data[1].State.ShouldBe(ChannelState.Running);
            result.Data[1].ConnectionDetails.ShouldNotBeNull();
            result.Data[1].ConnectionDetails.Name.ShouldBe("127.0.0.0:72368 -> 127.0.0.0:5672");
            result.Data[1].ConnectionDetails.PeerHost.ShouldBe("127.0.0.0");
            result.Data[1].ConnectionDetails.PeerPort.ShouldBe(98343);
            result.Data[1].OperationStats.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesConfirmedDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesNotRoutedDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessageGetDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data[1].OperationStats.TotalMessagesConfirmed.ShouldBe<ulong>(3150);
            result.Data[1].OperationStats.TotalMessagesPublished.ShouldBe<ulong>(3150);
            result.Data[1].OperationStats.TotalMessagesAcknowledged.ShouldBe<ulong>(107974);
            result.Data[1].OperationStats.TotalMessagesDelivered.ShouldBe<ulong>(107974);
            result.Data[1].OperationStats.TotalMessageDeliveryGets.ShouldBe<ulong>(107974);
            result.Data[1].OperationStats.TotalMessagesNotRouted.ShouldBe<ulong>(0);
            result.Data[1].OperationStats.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data[1].OperationStats.TotalMessageGets.ShouldBe<ulong>(0);
            result.Data[1].OperationStats.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data[1].OperationStats.TotalMessagesRedelivered.ShouldBe<ulong>(3);
            result.Data[1].OperationStats.MessagesConfirmedDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesNotRoutedDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesPublishedDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageDeliveryGetDetails?.Value.ShouldBe(1463.8M);
            result.Data[1].OperationStats.MessageDeliveryDetails?.Value.ShouldBe(1463.8M);
            result.Data[1].OperationStats.MessagesAcknowledgedDetails?.Value.ShouldBe(1473.0M);
            result.Data[1].OperationStats.MessagesDeliveredWithoutAckDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageGetDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageGetsWithoutAckDetails?.Value.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesRedeliveredDetails?.Value.ShouldBe(0.0M);
        }
    }
}