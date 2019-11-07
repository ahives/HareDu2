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
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_vhosts()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll();
            
            Assert.IsTrue(result.HasData);
            Assert.AreEqual(3, result.Data.Count);
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("TestVirtualHost", result.Data[2].Name);
            Assert.AreEqual(301363575, result.Data[2].PacketBytesReceived);
            Assert.IsNotNull(result.Data[2].RateOfPacketBytesReceived);
            Assert.AreEqual(0.0, result.Data[2].RateOfPacketBytesReceived?.Rate);
            Assert.AreEqual(368933935, result.Data[2].PacketBytesSent);
            Assert.IsNotNull(result.Data[2].RateOfPacketBytesSent);
            Assert.AreEqual(0.0, result.Data[2].RateOfPacketBytesSent?.Rate);
            Assert.AreEqual(0, result.Data[2].TotalMessages);
            Assert.IsNotNull(result.Data[2].RateOfMessages);
            Assert.AreEqual(0.0, result.Data[2].RateOfMessages?.Rate);
            Assert.AreEqual(0, result.Data[2].ReadyMessages);
            Assert.IsNotNull(result.Data[2].RateOfReadyMessages);
            Assert.AreEqual(0.0, result.Data[2].RateOfReadyMessages?.Rate);
            Assert.IsNotNull(result.Data[2].MessageStats);
            Assert.AreEqual(3, result.Data[2].MessageStats?.TotalMessageGets);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessageGetDetails);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessageGetDetails?.Rate);
            Assert.IsNotNull(result.Data[2].MessageStats);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessagesConfirmedDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessagesPublishedDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.UnroutableMessagesDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessagesAcknowledgedDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessageDeliveryDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessageDeliveryGetDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessageGetDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessageGetsWithoutAckDetails);
            Assert.IsNotNull(result.Data[2].MessageStats?.MessagesRedeliveredDetails);
            Assert.AreEqual(300000, result.Data[2].MessageStats?.TotalMessagesConfirmed);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessagesConfirmedDetails?.Rate);
            Assert.AreEqual(300000, result.Data[2].MessageStats?.TotalMessagesPublished);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessagesPublishedDetails?.Rate);
            Assert.AreEqual(0, result.Data[2].MessageStats?.TotalUnroutableMessages);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.UnroutableMessagesDetails?.Rate);
            Assert.AreEqual(300000, result.Data[2].MessageStats?.TotalMessagesAcknowledged);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessagesAcknowledgedDetails?.Rate);
            Assert.AreEqual(300000, result.Data[2].MessageStats?.TotalMessagesDelivered);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessageDeliveryDetails?.Rate);
            Assert.AreEqual(300003, result.Data[2].MessageStats?.TotalMessageDeliveryGets);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessageDeliveryGetDetails?.Rate);
            Assert.AreEqual(0, result.Data[2].MessageStats?.TotalMessageDeliveredWithoutAck);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate);
            Assert.AreEqual(3, result.Data[2].MessageStats?.TotalMessageGets);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessageGetDetails?.Rate);
            Assert.AreEqual(0, result.Data[2].MessageStats?.TotalMessageGetsWithoutAck);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessageGetsWithoutAckDetails?.Rate);
            Assert.AreEqual(3, result.Data[2].MessageStats?.TotalMessagesRedelivered);
            Assert.AreEqual(0.0, result.Data[2].MessageStats?.MessagesRedeliveredDetails?.Rate);
        }

        [Test]
        public async Task Verify_Create_works()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu7");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                });
            
            VirtualHostDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostDefinition>();

//            Assert.AreEqual(, definition.Tracing);
        }

        [Test]
        public async Task Verify_Delete_works()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"));

            Console.WriteLine((string) result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_start_vhost()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("", x => x.On(""));
            
            Console.WriteLine((string) result.ToJsonString());
        }
    }
}