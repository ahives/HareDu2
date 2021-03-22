namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using Core.Extensions;
    using HareDu.Extensions;
    using HareDu.Registration;
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
            result.Data[1].State.ShouldBe("running");
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

        [Test]
        public async Task Verify_can_get_all_channels1()
        {
            var services = GetContainerBuilder("TestData/ChannelInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(2, result.Data.Count);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.Data[0].Confirm);
                Assert.AreEqual(0, result.Data[0].UncommittedAcknowledgements);
                Assert.AreEqual(8, result.Data[0].GlobalPrefetchCount);
                Assert.AreEqual(0, result.Data[0].UnacknowledgedMessages);
                Assert.AreEqual(0, result.Data[0].UncommittedMessages);
                Assert.AreEqual(0, result.Data[0].UnconfirmedMessages);
                Assert.AreEqual(0, result.Data[0].PrefetchCount);
                Assert.AreEqual(1, result.Data[0].Number);
                Assert.AreEqual(6149, result.Data[0].TotalReductions);
                Assert.IsNotNull(result.Data[0].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[0].ReductionDetails?.Value);
                Assert.IsNotNull(result.Data[0].ConnectionDetails);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672", result.Data[0].ConnectionDetails?.Name);
                Assert.AreEqual("127.0.0.0", result.Data[0].ConnectionDetails?.PeerHost);
                Assert.AreEqual(98343, result.Data[0].ConnectionDetails?.PeerPort);
                Assert.IsFalse(result.Data[0].Transactional);
                Assert.IsNull(result.Data[0].OperationStats);
                Assert.IsNotNull(result.Data[1].OperationStats);
                Assert.IsTrue(result.Data[1].Confirm);
                Assert.AreEqual(0, result.Data[1].UncommittedAcknowledgements);
                Assert.AreEqual(64, result.Data[1].GlobalPrefetchCount);
                Assert.AreEqual(0, result.Data[1].UnacknowledgedMessages);
                Assert.AreEqual(0, result.Data[1].UncommittedMessages);
                Assert.AreEqual(0, result.Data[1].UnconfirmedMessages);
                Assert.AreEqual(0, result.Data[1].PrefetchCount);
                Assert.AreEqual(2, result.Data[1].Number);
                Assert.AreEqual(19755338, result.Data[1].TotalReductions);
                Assert.IsFalse(result.Data[1].Transactional);
                Assert.IsNotNull(result.Data[1].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[1].ReductionDetails.Value);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672 (2)", result.Data[1].Name);
                Assert.AreEqual("rabbit@localhost", result.Data[1].Node);
                Assert.AreEqual("guest", result.Data[1].User);
                Assert.AreEqual("guest", result.Data[1].UserWhoPerformedAction);
                Assert.AreEqual("TestVirtualHost", result.Data[1].VirtualHost);
                Assert.AreEqual("running", result.Data[1].State);
                Assert.IsNotNull(result.Data[1].ConnectionDetails);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672", result.Data[1].ConnectionDetails.Name);
                Assert.AreEqual("127.0.0.0", result.Data[1].ConnectionDetails.PeerHost);
                Assert.AreEqual(98343, result.Data[1].ConnectionDetails.PeerPort);
                Assert.IsNotNull(result.Data[1].OperationStats);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesConfirmedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesPublishedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesNotRoutedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesAcknowledgedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageDeliveryDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageDeliveryGetDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesDeliveredWithoutAckDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageGetDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageGetsWithoutAckDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesRedeliveredDetails);
                Assert.AreEqual(3150, result.Data[1].OperationStats?.TotalMessagesConfirmed);
                Assert.AreEqual(3150, result.Data[1].OperationStats?.TotalMessagesPublished);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesAcknowledged);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesDelivered);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessageDeliveryGets);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessagesNotRouted);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageDeliveredWithoutAck);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGets);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGetsWithoutAck);
                Assert.AreEqual(3, result.Data[1].OperationStats?.TotalMessagesRedelivered);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesConfirmedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesNotRoutedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesPublishedDetails?.Value);
                Assert.AreEqual(1463.8M, result.Data[1].OperationStats.MessageDeliveryGetDetails?.Value);
                Assert.AreEqual(1463.8M, result.Data[1].OperationStats.MessageDeliveryDetails?.Value);
                Assert.AreEqual(1473.0M, result.Data[1].OperationStats.MessagesAcknowledgedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessagesDeliveredWithoutAckDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessageGetDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessageGetsWithoutAckDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessagesRedeliveredDetails?.Value);
            });
        }

        [Test]
        public async Task Verify_can_get_all_channels2()
        {
            var services = GetContainerBuilder("TestData/ChannelInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllChannels();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(2, result.Data.Count);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.Data[0].Confirm);
                Assert.AreEqual(0, result.Data[0].UncommittedAcknowledgements);
                Assert.AreEqual(8, result.Data[0].GlobalPrefetchCount);
                Assert.AreEqual(0, result.Data[0].UnacknowledgedMessages);
                Assert.AreEqual(0, result.Data[0].UncommittedMessages);
                Assert.AreEqual(0, result.Data[0].UnconfirmedMessages);
                Assert.AreEqual(0, result.Data[0].PrefetchCount);
                Assert.AreEqual(1, result.Data[0].Number);
                Assert.AreEqual(6149, result.Data[0].TotalReductions);
                Assert.IsNotNull(result.Data[0].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[0].ReductionDetails?.Value);
                Assert.IsNotNull(result.Data[0].ConnectionDetails);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672", result.Data[0].ConnectionDetails?.Name);
                Assert.AreEqual("127.0.0.0", result.Data[0].ConnectionDetails?.PeerHost);
                Assert.AreEqual(98343, result.Data[0].ConnectionDetails?.PeerPort);
                Assert.IsFalse(result.Data[0].Transactional);
                Assert.IsNull(result.Data[0].OperationStats);
                Assert.IsNotNull(result.Data[1].OperationStats);
                Assert.IsTrue(result.Data[1].Confirm);
                Assert.AreEqual(0, result.Data[1].UncommittedAcknowledgements);
                Assert.AreEqual(64, result.Data[1].GlobalPrefetchCount);
                Assert.AreEqual(0, result.Data[1].UnacknowledgedMessages);
                Assert.AreEqual(0, result.Data[1].UncommittedMessages);
                Assert.AreEqual(0, result.Data[1].UnconfirmedMessages);
                Assert.AreEqual(0, result.Data[1].PrefetchCount);
                Assert.AreEqual(2, result.Data[1].Number);
                Assert.AreEqual(19755338, result.Data[1].TotalReductions);
                Assert.IsFalse(result.Data[1].Transactional);
                Assert.IsNotNull(result.Data[1].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[1].ReductionDetails.Value);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672 (2)", result.Data[1].Name);
                Assert.AreEqual("rabbit@localhost", result.Data[1].Node);
                Assert.AreEqual("guest", result.Data[1].User);
                Assert.AreEqual("guest", result.Data[1].UserWhoPerformedAction);
                Assert.AreEqual("TestVirtualHost", result.Data[1].VirtualHost);
                Assert.AreEqual("running", result.Data[1].State);
                Assert.IsNotNull(result.Data[1].ConnectionDetails);
                Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672", result.Data[1].ConnectionDetails.Name);
                Assert.AreEqual("127.0.0.0", result.Data[1].ConnectionDetails.PeerHost);
                Assert.AreEqual(98343, result.Data[1].ConnectionDetails.PeerPort);
                Assert.IsNotNull(result.Data[1].OperationStats);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesConfirmedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesPublishedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesNotRoutedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesAcknowledgedDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageDeliveryDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageDeliveryGetDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesDeliveredWithoutAckDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageGetDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessageGetsWithoutAckDetails);
                Assert.IsNotNull(result.Data[1].OperationStats?.MessagesRedeliveredDetails);
                Assert.AreEqual(3150, result.Data[1].OperationStats?.TotalMessagesConfirmed);
                Assert.AreEqual(3150, result.Data[1].OperationStats?.TotalMessagesPublished);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesAcknowledged);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesDelivered);
                Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessageDeliveryGets);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessagesNotRouted);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageDeliveredWithoutAck);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGets);
                Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGetsWithoutAck);
                Assert.AreEqual(3, result.Data[1].OperationStats?.TotalMessagesRedelivered);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesConfirmedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesNotRoutedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats?.MessagesPublishedDetails?.Value);
                Assert.AreEqual(1463.8M, result.Data[1].OperationStats.MessageDeliveryGetDetails?.Value);
                Assert.AreEqual(1463.8M, result.Data[1].OperationStats.MessageDeliveryDetails?.Value);
                Assert.AreEqual(1473.0M, result.Data[1].OperationStats.MessagesAcknowledgedDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessagesDeliveredWithoutAckDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessageGetDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessageGetsWithoutAckDetails?.Value);
                Assert.AreEqual(0.0M, result.Data[1].OperationStats.MessagesRedeliveredDetails?.Value);
            });
        }
    }
}