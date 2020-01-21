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
    using Shouldly;

    [TestFixture]
    public class ChannelTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/ChannelInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll();
            
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
            result.Data[0].ReductionDetails?.Rate.ShouldBe(0.0M);
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
            result.Data[1].ReductionDetails?.Rate.ShouldBe(0.0M);
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
            result.Data[1].OperationStats.MessagesConfirmedDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesNotRoutedDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesPublishedDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageDeliveryGetDetails?.Rate.ShouldBe(1463.8M);
            result.Data[1].OperationStats.MessageDeliveryDetails?.Rate.ShouldBe(1463.8M);
            result.Data[1].OperationStats.MessagesAcknowledgedDetails?.Rate.ShouldBe(1473.0M);
            result.Data[1].OperationStats.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[1].OperationStats.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
        }
    }
}