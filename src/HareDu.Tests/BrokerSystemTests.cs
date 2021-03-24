namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    [TestFixture]
    public class BrokerSystemTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_get_system_overview1()
        {
            var services = GetContainerBuilder("TestData/NewSystemOverviewInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<BrokerSystem>()
                .GetSystemOverview();

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data.ObjectTotals);
                Assert.IsNotNull(result.Data.QueueStats);
                Assert.IsNotNull(result.Data.MessageStats);
                Assert.IsNotNull(result.Data.ChurnRates);
                Assert.AreEqual("rabbit@haredu", result.Data.ClusterName);
                Assert.AreEqual("23.2", result.Data.ErlangVersion);
                Assert.AreEqual("Erlang/OTP 23 [erts-11.1.4] [source] [64-bit] [smp:4:4] [ds:4:4:10] [async-threads:64] [hipe] [dtrace]", result.Data.ErlangFullVersion);
                Assert.AreEqual("rabbit@localhost", result.Data.Node);
                Assert.AreEqual(3, result.Data.ObjectTotals?.TotalChannels);
                Assert.AreEqual(3, result.Data.ObjectTotals?.TotalConsumers);
                Assert.AreEqual(2, result.Data.ObjectTotals?.TotalConnections);
                Assert.AreEqual(100, result.Data.ObjectTotals?.TotalExchanges);
                Assert.AreEqual(11, result.Data.ObjectTotals?.TotalQueues);
                Assert.AreEqual(7, result.Data.MessageStats?.TotalMessageGets);
                Assert.IsNotNull(result.Data.MessageStats?.MessageGetDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageGetDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesAcknowledged);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesAcknowledgedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesAcknowledgedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesConfirmed);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesConfirmedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesConfirmedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageDeliveryDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesRedeliveredDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesRedeliveredDetails?.Value);
                Assert.AreEqual(7, result.Data.MessageStats?.TotalMessagesRedelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesPublishedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesPublishedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesPublished);
                Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryGetDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageDeliveryGetDetails?.Value);
                Assert.AreEqual(200007, result.Data.MessageStats?.TotalMessageDeliveryGets);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageDeliveredWithoutAck);
                Assert.IsNotNull(result.Data.MessageStats?.MessageGetsWithoutAckDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageGetsWithoutAckDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageGetsWithoutAck);
                Assert.IsNotNull(result.Data.MessageStats?.UnroutableMessagesDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.UnroutableMessagesDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalUnroutableMessages);
                Assert.IsNotNull(result.Data.MessageStats?.DiskReadDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.DiskReadDetails?.Value);
                Assert.AreEqual(8734, result.Data.MessageStats?.TotalDiskReads);
                Assert.IsNotNull(result.Data.MessageStats?.DiskWriteDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.DiskWriteDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalDiskWrites);
                Assert.IsNotNull(result.Data.QueueStats);
                Assert.IsNotNull(result.Data.QueueStats?.MessageDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.MessageDetails?.Value);
                Assert.AreEqual(3, result.Data.QueueStats?.TotalMessages);
                Assert.IsNotNull(result.Data.QueueStats?.UnacknowledgedDeliveredMessagesDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.UnacknowledgedDeliveredMessagesDetails?.Value);
                Assert.AreEqual(0, result.Data.QueueStats?.TotalUnacknowledgedDeliveredMessages);
                Assert.IsNotNull(result.Data.QueueStats?.MessagesReadyForDeliveryDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.MessagesReadyForDeliveryDetails?.Value);
                Assert.AreEqual(3, result.Data.QueueStats?.TotalMessagesReadyForDelivery);
                Assert.IsNotNull(result.Data.ChurnRates);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedChannelDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedChannelDetails?.Value);
                Assert.AreEqual(52, result.Data.ChurnRates?.TotalChannelsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedChannelDetails);
                Assert.AreEqual(1.4M, result.Data.ChurnRates?.CreatedChannelDetails?.Value);
                Assert.AreEqual(61, result.Data.ChurnRates?.TotalChannelsCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedConnectionDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedConnectionDetails?.Value);
                Assert.AreEqual(12, result.Data.ChurnRates?.TotalConnectionsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedConnectionDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedConnectionDetails?.Value);
                Assert.AreEqual(12, result.Data.ChurnRates?.TotalConnectionsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedConnectionDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.CreatedConnectionDetails?.Value);
                Assert.AreEqual(14, result.Data.ChurnRates?.TotalConnectionsCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedQueueDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.CreatedQueueDetails?.Value);
                Assert.AreEqual(8, result.Data.ChurnRates?.TotalQueuesCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.DeclaredQueueDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.DeclaredQueueDetails?.Value);
                Assert.AreEqual(10, result.Data.ChurnRates?.TotalQueuesDeclared);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.AreEqual(5, result.Data.ChurnRates?.TotalQueuesDeleted);
                Assert.AreEqual("3.8.9", result.Data.RabbitMqVersion);
                Assert.AreEqual("3.8.9", result.Data.ManagementVersion);
                Assert.AreEqual("basic", result.Data.RatesMode);
                Assert.AreEqual(4, result.Data.ExchangeTypes.Count);
            });
        }

        [Test]
        public async Task Verify_can_get_system_overview2()
        {
            var services = GetContainerBuilder("TestData/NewSystemOverviewInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetBrokerSystemOverview();

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data.ObjectTotals);
                Assert.IsNotNull(result.Data.QueueStats);
                Assert.IsNotNull(result.Data.MessageStats);
                Assert.IsNotNull(result.Data.ChurnRates);
                Assert.AreEqual("rabbit@haredu", result.Data.ClusterName);
                Assert.AreEqual("23.2", result.Data.ErlangVersion);
                Assert.AreEqual("Erlang/OTP 23 [erts-11.1.4] [source] [64-bit] [smp:4:4] [ds:4:4:10] [async-threads:64] [hipe] [dtrace]", result.Data.ErlangFullVersion);
                Assert.AreEqual("rabbit@localhost", result.Data.Node);
                Assert.AreEqual(3, result.Data.ObjectTotals?.TotalChannels);
                Assert.AreEqual(3, result.Data.ObjectTotals?.TotalConsumers);
                Assert.AreEqual(2, result.Data.ObjectTotals?.TotalConnections);
                Assert.AreEqual(100, result.Data.ObjectTotals?.TotalExchanges);
                Assert.AreEqual(11, result.Data.ObjectTotals?.TotalQueues);
                Assert.AreEqual(7, result.Data.MessageStats?.TotalMessageGets);
                Assert.IsNotNull(result.Data.MessageStats?.MessageGetDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageGetDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesAcknowledged);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesAcknowledgedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesAcknowledgedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesConfirmed);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesConfirmedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesConfirmedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageDeliveryDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesRedeliveredDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesRedeliveredDetails?.Value);
                Assert.AreEqual(7, result.Data.MessageStats?.TotalMessagesRedelivered);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesPublishedDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesPublishedDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesPublished);
                Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryGetDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageDeliveryGetDetails?.Value);
                Assert.AreEqual(200007, result.Data.MessageStats?.TotalMessageDeliveryGets);
                Assert.IsNotNull(result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageDeliveredWithoutAck);
                Assert.IsNotNull(result.Data.MessageStats?.MessageGetsWithoutAckDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.MessageGetsWithoutAckDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageGetsWithoutAck);
                Assert.IsNotNull(result.Data.MessageStats?.UnroutableMessagesDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.UnroutableMessagesDetails?.Value);
                Assert.AreEqual(0, result.Data.MessageStats?.TotalUnroutableMessages);
                Assert.IsNotNull(result.Data.MessageStats?.DiskReadDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.DiskReadDetails?.Value);
                Assert.AreEqual(8734, result.Data.MessageStats?.TotalDiskReads);
                Assert.IsNotNull(result.Data.MessageStats?.DiskWriteDetails);
                Assert.AreEqual(0.0M, result.Data.MessageStats?.DiskWriteDetails?.Value);
                Assert.AreEqual(200000, result.Data.MessageStats?.TotalDiskWrites);
                Assert.IsNotNull(result.Data.QueueStats);
                Assert.IsNotNull(result.Data.QueueStats?.MessageDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.MessageDetails?.Value);
                Assert.AreEqual(3, result.Data.QueueStats?.TotalMessages);
                Assert.IsNotNull(result.Data.QueueStats?.UnacknowledgedDeliveredMessagesDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.UnacknowledgedDeliveredMessagesDetails?.Value);
                Assert.AreEqual(0, result.Data.QueueStats?.TotalUnacknowledgedDeliveredMessages);
                Assert.IsNotNull(result.Data.QueueStats?.MessagesReadyForDeliveryDetails);
                Assert.AreEqual(0.0M, result.Data.QueueStats?.MessagesReadyForDeliveryDetails?.Value);
                Assert.AreEqual(3, result.Data.QueueStats?.TotalMessagesReadyForDelivery);
                Assert.IsNotNull(result.Data.ChurnRates);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedChannelDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedChannelDetails?.Value);
                Assert.AreEqual(52, result.Data.ChurnRates?.TotalChannelsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedChannelDetails);
                Assert.AreEqual(1.4M, result.Data.ChurnRates?.CreatedChannelDetails?.Value);
                Assert.AreEqual(61, result.Data.ChurnRates?.TotalChannelsCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedConnectionDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedConnectionDetails?.Value);
                Assert.AreEqual(12, result.Data.ChurnRates?.TotalConnectionsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.ClosedConnectionDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.ClosedConnectionDetails?.Value);
                Assert.AreEqual(12, result.Data.ChurnRates?.TotalConnectionsClosed);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedConnectionDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.CreatedConnectionDetails?.Value);
                Assert.AreEqual(14, result.Data.ChurnRates?.TotalConnectionsCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.CreatedQueueDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.CreatedQueueDetails?.Value);
                Assert.AreEqual(8, result.Data.ChurnRates?.TotalQueuesCreated);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.IsNotNull(result.Data.ChurnRates?.DeclaredQueueDetails);
                Assert.AreEqual(0.2M, result.Data.ChurnRates?.DeclaredQueueDetails?.Value);
                Assert.AreEqual(10, result.Data.ChurnRates?.TotalQueuesDeclared);
                Assert.IsNotNull(result.Data.ChurnRates?.DeletedQueueDetails);
                Assert.AreEqual(0.0M, result.Data.ChurnRates?.DeletedQueueDetails?.Value);
                Assert.AreEqual(5, result.Data.ChurnRates?.TotalQueuesDeleted);
                Assert.AreEqual("3.8.9", result.Data.RabbitMqVersion);
                Assert.AreEqual("3.8.9", result.Data.ManagementVersion);
                Assert.AreEqual("basic", result.Data.RatesMode);
                Assert.AreEqual(4, result.Data.ExchangeTypes.Count);
            });
        }
    }
}