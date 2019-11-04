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
    public class ChannelTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/ChannelInfo1.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll();
            
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
            Assert.AreEqual(0.0, result.Data[0].ReductionDetails?.Rate);
            Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672 (1)", result.Data[0].Name);
            Assert.AreEqual("rabbit@localhost", result.Data[0].Node);
            Assert.AreEqual("guest", result.Data[0].User);
            Assert.AreEqual("guest", result.Data[0].UserWhoPerformedAction);
            Assert.AreEqual("TestVirtualHost", result.Data[0].VirtualHost);
            Assert.AreEqual("running", result.Data[0].State);
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
            Assert.IsNotNull(result.Data[1].ReductionDetails);
            Assert.IsFalse(result.Data[1].Transactional);
            Assert.AreEqual(0.0, result.Data[1].ReductionDetails?.Rate);
            Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672 (2)", result.Data[1].Name);
            Assert.AreEqual("rabbit@localhost", result.Data[1].Node);
            Assert.AreEqual("guest", result.Data[1].User);
            Assert.AreEqual("guest", result.Data[1].UserWhoPerformedAction);
            Assert.AreEqual("TestVirtualHost", result.Data[1].VirtualHost);
            Assert.AreEqual("running", result.Data[1].State);
            Assert.IsNotNull(result.Data[1].ConnectionDetails);
            Assert.AreEqual("127.0.0.0:72368 -> 127.0.0.0:5672", result.Data[1].ConnectionDetails?.Name);
            Assert.AreEqual("127.0.0.0", result.Data[1].ConnectionDetails?.PeerHost);
            Assert.AreEqual(98343, result.Data[1].ConnectionDetails?.PeerPort);
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
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessagesConfirmedDetails?.Rate);
            Assert.AreEqual(3150, result.Data[1].OperationStats?.TotalMessagesPublished);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessagesPublishedDetails?.Rate);
            Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessagesNotRouted);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessagesNotRoutedDetails?.Rate);
            Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesAcknowledged);
            Assert.AreEqual(1473.0, result.Data[1].OperationStats?.MessagesAcknowledgedDetails?.Rate);
            Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessagesDelivered);
            Assert.AreEqual(1463.8, result.Data[1].OperationStats?.MessageDeliveryDetails?.Rate);
            Assert.AreEqual(107974, result.Data[1].OperationStats?.TotalMessageDeliveryGets);
            Assert.AreEqual(1463.8, result.Data[1].OperationStats?.MessageDeliveryGetDetails?.Rate);
            Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageDeliveredWithoutAck);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessagesDeliveredWithoutAckDetails?.Rate);
            Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGets);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessageGetDetails?.Rate);
            Assert.AreEqual(0, result.Data[1].OperationStats?.TotalMessageGetsWithoutAck);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessageGetsWithoutAckDetails?.Rate);
            Assert.AreEqual(3, result.Data[1].OperationStats?.TotalMessagesRedelivered);
            Assert.AreEqual(0.0, result.Data[1].OperationStats?.MessagesRedeliveredDetails?.Rate);
        }
    }
}