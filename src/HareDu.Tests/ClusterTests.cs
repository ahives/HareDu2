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
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/ClusterInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Cluster>()
                .GetDetails();

            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data.ObjectTotals);
            Assert.IsNotNull(result.Data.QueueStats);
            Assert.IsNotNull(result.Data.MessageStats);
            Assert.IsNotNull(result.Data.ChurnRates);
            Assert.AreEqual("rabbit@haredu", result.Data.ClusterName);
            Assert.AreEqual("22.0.4", result.Data.ErlangVersion);
            Assert.AreEqual("Erlang/OTP 22 [erts-10.4.3] [source] [64-bit] [smp:4:4] [ds:4:4:10] [async-threads:64] [hipe] [dtrace]", result.Data.ErlangFullVersion);
            Assert.AreEqual("rabbit@localhost", result.Data.Node);
            Assert.AreEqual(3, result.Data.ObjectTotals?.TotalChannels);
            Assert.AreEqual(3, result.Data.ObjectTotals?.TotalConsumers);
            Assert.AreEqual(2, result.Data.ObjectTotals?.TotalConnections);
            Assert.AreEqual(100, result.Data.ObjectTotals?.TotalExchanges);
            Assert.AreEqual(11, result.Data.ObjectTotals?.TotalQueues);
            Assert.AreEqual(7, result.Data.MessageStats?.TotalMessageGets);
            Assert.IsNotNull(result.Data.MessageStats?.MessageGetDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessageGetDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesAcknowledged);
            Assert.IsNotNull(result.Data.MessageStats?.MessagesAcknowledgedDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessagesAcknowledgedDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesConfirmed);
            Assert.IsNotNull(result.Data.MessageStats?.MessagesConfirmedDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessagesConfirmedDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
            Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessageDeliveryDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesDelivered);
            Assert.IsNotNull(result.Data.MessageStats?.MessagesRedeliveredDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessagesRedeliveredDetails?.Rate);
            Assert.AreEqual(7, result.Data.MessageStats?.TotalMessagesRedelivered);
            Assert.IsNotNull(result.Data.MessageStats?.MessagesPublishedDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessagesPublishedDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalMessagesPublished);
            Assert.IsNotNull(result.Data.MessageStats?.MessageDeliveryGetDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessageDeliveryGetDetails?.Rate);
            Assert.AreEqual(200007, result.Data.MessageStats?.TotalMessageDeliveryGets);
            Assert.IsNotNull(result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate);
            Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageDeliveredWithoutAck);
            Assert.IsNotNull(result.Data.MessageStats?.MessageGetsWithoutAckDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.MessageGetsWithoutAckDetails?.Rate);
            Assert.AreEqual(0, result.Data.MessageStats?.TotalMessageGetsWithoutAck);
            Assert.IsNotNull(result.Data.MessageStats?.UnroutableMessagesDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.UnroutableMessagesDetails?.Rate);
            Assert.AreEqual(0, result.Data.MessageStats?.TotalUnroutableMessages);
            Assert.IsNotNull(result.Data.MessageStats?.DiskReadDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.DiskReadDetails?.Rate);
            Assert.AreEqual(8734, result.Data.MessageStats?.TotalDiskReads);
            Assert.IsNotNull(result.Data.MessageStats?.DiskWriteDetails);
            Assert.AreEqual(0.0, result.Data.MessageStats?.DiskWriteDetails?.Rate);
            Assert.AreEqual(200000, result.Data.MessageStats?.TotalDiskWrites);
            Assert.IsNotNull(result.Data.QueueStats);
            Assert.IsNotNull(result.Data.QueueStats.RateOfMessages);
            Assert.AreEqual(0.0, result.Data.QueueStats.RateOfMessages?.Rate);
            Assert.AreEqual(3, result.Data.QueueStats?.TotalMessages);
            Assert.IsNotNull(result.Data.QueueStats.RateOfUnacknowledgedDeliveredMessages);
            Assert.AreEqual(0, result.Data.QueueStats.RateOfUnacknowledgedDeliveredMessages?.Rate);
            Assert.AreEqual(0.0, result.Data.QueueStats?.TotalUnacknowledgedDeliveredMessages);
            Assert.IsNotNull(result.Data.QueueStats.RateOfMessagesReadyForDelivery);
            Assert.AreEqual(0.0, result.Data.QueueStats.RateOfMessagesReadyForDelivery?.Rate);
            Assert.AreEqual(3, result.Data.QueueStats?.TotalMessagesReadyForDelivery);
            Assert.IsNotNull(result.Data.ChurnRates);
            Assert.IsNotNull(result.Data.ChurnRates.ClosedChannelDetails);
            Assert.AreEqual(0.0, result.Data.ChurnRates?.ClosedChannelDetails?.Rate);
            Assert.AreEqual(52, result.Data.ChurnRates.TotalChannelsClosed);
            Assert.IsNotNull(result.Data.ChurnRates.CreatedChannelDetails);
            Assert.AreEqual(1.4, result.Data.ChurnRates?.CreatedChannelDetails?.Rate);
            Assert.AreEqual(61, result.Data.ChurnRates.TotalChannelsCreated);
            Assert.IsNotNull(result.Data.ChurnRates.ClosedConnectionDetails);
            Assert.AreEqual(0.0, result.Data.ChurnRates?.ClosedConnectionDetails?.Rate);
            Assert.AreEqual(12, result.Data.ChurnRates.TotalConnectionsClosed);
            Assert.IsNotNull(result.Data.ChurnRates.ClosedConnectionDetails);
            Assert.AreEqual(0.0, result.Data.ChurnRates?.ClosedConnectionDetails?.Rate);
            Assert.AreEqual(12, result.Data.ChurnRates.TotalConnectionsClosed);
            Assert.IsNotNull(result.Data.ChurnRates.CreatedConnectionDetails);
            Assert.AreEqual(0.2, result.Data.ChurnRates?.CreatedConnectionDetails?.Rate);
            Assert.AreEqual(14, result.Data.ChurnRates.TotalConnectionsCreated);
            Assert.IsNotNull(result.Data.ChurnRates.CreatedQueueDetails);
            Assert.AreEqual(0.2, result.Data.ChurnRates?.CreatedQueueDetails?.Rate);
            Assert.AreEqual(8, result.Data.ChurnRates.TotalQueuesCreated);
            Assert.IsNotNull(result.Data.ChurnRates.DeclaredQueueDetails);
            Assert.AreEqual(0.2, result.Data.ChurnRates?.DeclaredQueueDetails?.Rate);
            Assert.AreEqual(10, result.Data.ChurnRates.TotalQueuesDeclared);
            Assert.IsNotNull(result.Data.ChurnRates.DeletedQueueDetails);
            Assert.AreEqual(0.0, result.Data.ChurnRates?.DeletedQueueDetails?.Rate);
            Assert.AreEqual(5, result.Data.ChurnRates.TotalQueuesDeleted);
            Assert.AreEqual("3.7.15", result.Data.RabbitMqVersion);
            Assert.AreEqual("3.7.15", result.Data.ManagementVersion);
            Assert.AreEqual("basic", result.Data.RatesMode);
            Assert.AreEqual(4, result.Data.ExchangeTypes.Count);
//            Assert.AreEqual("", result.Data.ExchangeTypes.Count);
//            Assert.AreEqual("", result.Data);
        }
    }
}