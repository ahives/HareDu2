namespace HareDu.Tests
{
    using Core.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class SystemOverviewTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_system_overview()
        {
            var container = GetContainerBuilder("TestData/SystemOverviewInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<SystemOverview>()
                .Get()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ObjectTotals.ShouldNotBeNull();
            result.Data.QueueStats.ShouldNotBeNull();
            result.Data.MessageStats.ShouldNotBeNull();
            result.Data.ChurnRates.ShouldNotBeNull();
            result.Data.ClusterName.ShouldBe("rabbit@haredu");
            result.Data.ErlangVersion.ShouldBe("22.0.4");
            result.Data.ErlangFullVersion.ShouldBe("Erlang/OTP 22 [erts-10.4.3] [source] [64-bit] [smp:4:4] [ds:4:4:10] [async-threads:64] [hipe] [dtrace]");
            result.Data.Node.ShouldBe("rabbit@localhost");
            result.Data.ObjectTotals?.TotalChannels.ShouldBe<ulong>(3);
            result.Data.ObjectTotals?.TotalConsumers.ShouldBe<ulong>(3);
            result.Data.ObjectTotals?.TotalConnections.ShouldBe<ulong>(2);
            result.Data.ObjectTotals?.TotalExchanges.ShouldBe<ulong>(100);
            result.Data.ObjectTotals?.TotalQueues.ShouldBe<ulong>(11);
            result.Data.MessageStats?.TotalMessageGets.ShouldBe<ulong>(7);
            result.Data.MessageStats.MessageGetDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageGetDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesAcknowledgedDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesConfirmed.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesConfirmedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesConfirmedDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageDeliveryDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesRedeliveredDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(7);
            result.Data.MessageStats.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesPublishedDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageDeliveryGetDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(200007);
            result.Data.MessageStats.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data.MessageStats.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageGetsWithoutAckDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data.MessageStats.UnroutableMessagesDetails.ShouldNotBeNull();
            result.Data.MessageStats?.UnroutableMessagesDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalUnroutableMessages.ShouldBe<ulong>(0);
            result.Data.MessageStats.DiskReadDetails.ShouldNotBeNull();
            result.Data.MessageStats?.DiskReadDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalDiskReads.ShouldBe<ulong>(8734);
            result.Data.MessageStats.DiskWriteDetails.ShouldNotBeNull();
            result.Data.MessageStats?.DiskWriteDetails?.Value.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalDiskWrites.ShouldBe<ulong>(200000);
            result.Data.QueueStats.ShouldNotBeNull();
            result.Data.QueueStats.MessageDetails.ShouldNotBeNull();
            result.Data.QueueStats?.MessageDetails?.Value.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalMessages.ShouldBe<ulong>(3);
            result.Data.QueueStats.UnacknowledgedDeliveredMessagesDetails.ShouldNotBeNull();
            result.Data.QueueStats?.UnacknowledgedDeliveredMessagesDetails?.Value.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalUnacknowledgedDeliveredMessages.ShouldBe<ulong>(0);
            result.Data.QueueStats.MessagesReadyForDeliveryDetails.ShouldNotBeNull();
            result.Data.QueueStats?.MessagesReadyForDeliveryDetails?.Value.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalMessagesReadyForDelivery.ShouldBe<ulong>(3);
            result.Data.ChurnRates.ShouldNotBeNull();
            result.Data.ChurnRates.ClosedChannelDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedChannelDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalChannelsClosed.ShouldBe<ulong>(52);
            result.Data.ChurnRates.CreatedChannelDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedChannelDetails?.Value.ShouldBe(1.4M);
            result.Data.ChurnRates.TotalChannelsCreated.ShouldBe<ulong>(61);
            result.Data.ChurnRates.ClosedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedConnectionDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalConnectionsClosed.ShouldBe<ulong>(12);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.ClosedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedConnectionDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalConnectionsClosed.ShouldBe<ulong>(12);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.CreatedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedConnectionDetails?.Value.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalConnectionsCreated.ShouldBe<ulong>(14);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.CreatedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedQueueDetails?.Value.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalQueuesCreated.ShouldBe<ulong>(8);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.DeclaredQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeclaredQueueDetails?.Value.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalQueuesDeclared.ShouldBe<ulong>(10);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Value.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalQueuesDeleted.ShouldBe<ulong>(5);
            result.Data.RabbitMqVersion.ShouldBe("3.7.15");
            result.Data.ManagementVersion.ShouldBe("3.7.15");
            result.Data.RatesMode.ShouldBe("basic");
            result.Data.ExchangeTypes.Count.ShouldBe(4);
//            Assert.AreEqual("", result.Data.ExchangeTypes.Count);
//            Assert.AreEqual("", result.Data);
        }
    }
}