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
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class SystemOverviewTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/SystemOverviewInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<SystemOverview>()
                .Get();

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
            result.Data.MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesAcknowledgedDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesConfirmed.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesConfirmedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesConfirmedDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageDeliveryDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(7);
            result.Data.MessageStats.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesPublishedDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(200000);
            result.Data.MessageStats.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageDeliveryGetDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(200007);
            result.Data.MessageStats.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data.MessageStats.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data.MessageStats?.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data.MessageStats.UnroutableMessagesDetails.ShouldNotBeNull();
            result.Data.MessageStats?.UnroutableMessagesDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalUnroutableMessages.ShouldBe<ulong>(0);
            result.Data.MessageStats.DiskReadDetails.ShouldNotBeNull();
            result.Data.MessageStats?.DiskReadDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalDiskReads.ShouldBe<ulong>(8734);
            result.Data.MessageStats.DiskWriteDetails.ShouldNotBeNull();
            result.Data.MessageStats?.DiskWriteDetails?.Rate.ShouldBe(0.0M);
            result.Data.MessageStats?.TotalDiskWrites.ShouldBe<ulong>(200000);
            result.Data.QueueStats.ShouldNotBeNull();
            result.Data.QueueStats.RateOfMessages.ShouldNotBeNull();
            result.Data.QueueStats?.RateOfMessages?.Rate.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalMessages.ShouldBe<ulong>(3);
            result.Data.QueueStats.RateOfUnacknowledgedDeliveredMessages.ShouldNotBeNull();
            result.Data.QueueStats?.RateOfUnacknowledgedDeliveredMessages?.Rate.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalUnacknowledgedDeliveredMessages.ShouldBe<ulong>(0);
            result.Data.QueueStats.RateOfMessagesReadyForDelivery.ShouldNotBeNull();
            result.Data.QueueStats?.RateOfMessagesReadyForDelivery?.Rate.ShouldBe(0.0M);
            result.Data.QueueStats?.TotalMessagesReadyForDelivery.ShouldBe<ulong>(3);
            result.Data.ChurnRates.ShouldNotBeNull();
            result.Data.ChurnRates.ClosedChannelDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedChannelDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalChannelsClosed.ShouldBe<ulong>(52);
            result.Data.ChurnRates.CreatedChannelDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedChannelDetails?.Rate.ShouldBe(1.4M);
            result.Data.ChurnRates.TotalChannelsCreated.ShouldBe<ulong>(61);
            result.Data.ChurnRates.ClosedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedConnectionDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalConnectionsClosed.ShouldBe<ulong>(12);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.ClosedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.ClosedConnectionDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.TotalConnectionsClosed.ShouldBe<ulong>(12);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.CreatedConnectionDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedConnectionDetails?.Rate.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalConnectionsCreated.ShouldBe<ulong>(14);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.CreatedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.CreatedQueueDetails?.Rate.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalQueuesCreated.ShouldBe<ulong>(8);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data.ChurnRates.DeclaredQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeclaredQueueDetails?.Rate.ShouldBe(0.2M);
            result.Data.ChurnRates.TotalQueuesDeclared.ShouldBe<ulong>(10);
            result.Data.ChurnRates.DeletedQueueDetails.ShouldNotBeNull();
            result.Data.ChurnRates?.DeletedQueueDetails?.Rate.ShouldBe(0.0M);
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