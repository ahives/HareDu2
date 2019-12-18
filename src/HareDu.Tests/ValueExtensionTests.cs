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
    using System.Linq;
    using Autofac;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ValueExtensionTests :
        HareDuTesting
    {
        [Test]
        public void Verify_Where_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            var vhosts = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "TestVirtualHost");

            vhosts.Count.ShouldBe(1);
            vhosts[0].Name.ShouldBe("TestVirtualHost");
            vhosts[0].PacketBytesReceived.ShouldBe<ulong>(301363575);
            vhosts[0].RateOfPacketBytesReceived.ShouldNotBeNull();
            vhosts[0].RateOfPacketBytesReceived?.Rate.ShouldBe(0.0M);
            vhosts[0].PacketBytesSent.ShouldBe<ulong>(368933935);
            vhosts[0].RateOfPacketBytesSent.ShouldNotBeNull();
            vhosts[0].RateOfPacketBytesSent?.Rate.ShouldBe(0.0M);
            vhosts[0].TotalMessages.ShouldBe<ulong>(0);
            vhosts[0].RateOfMessages.ShouldNotBeNull();
            vhosts[0].RateOfMessages?.Rate.ShouldBe(0.0M);
            vhosts[0].ReadyMessages.ShouldBe<ulong>(0);
            vhosts[0].RateOfReadyMessages.ShouldNotBeNull();
            vhosts[0].RateOfReadyMessages?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats.ShouldNotBeNull();
            vhosts[0].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            vhosts[0].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessagesConfirmedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessagesPublishedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.UnroutableMessagesDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessagesAcknowledgedDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessageDeliveryDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessageDeliveryGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.MessagesRedeliveredDetails.ShouldNotBeNull();
            vhosts[0].MessageStats?.TotalMessagesConfirmed.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats?.MessagesConfirmedDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats?.MessagesPublishedDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalUnroutableMessages.ShouldBe<ulong>(0);
            vhosts[0].MessageStats?.UnroutableMessagesDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats?.MessagesAcknowledgedDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(300000);
            vhosts[0].MessageStats?.MessageDeliveryDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(300003);
            vhosts[0].MessageStats?.MessageDeliveryGetDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            vhosts[0].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            vhosts[0].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            vhosts[0].MessageStats?.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            vhosts[0].MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(3);
            vhosts[0].MessageStats?.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
        }

        [Test]
        public void Verify_Unfold_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Unfold();
            
            result.HasData.ShouldBeTrue();
            result.HasFaulted.ShouldBeFalse();
            result.Data.Count.ShouldBe(3);
            result.Data[0].Name.ShouldBe("/");
            result.Data[1].Name.ShouldBe("HareDu");
            result.Data[2].Name.ShouldBe("TestVirtualHost");
        }

        [Test]
        public void Verify_Select_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Select(x => x.Data);

            result.Count.ShouldBe(3);
            result[0].Name.ShouldBe("/");
            result[1].Name.ShouldBe("HareDu");
            result[2].Name.ShouldBe("TestVirtualHost");
        }

        [Test]
        public void Verify_Any_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            bool found = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Any();
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_Any_with_predicate_works()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            bool found = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Any(x => x.Name == "HareDu");
            
            found.ShouldBeTrue();
        }
        
        [Test]
        public void Verify_FirstOrDefault_works()
        {
            var container = GetContainerBuilder("TestData/ExchangeInfo.json").Build();
            ExchangeInfo exchange = container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .GetAll()
                .Where(x => x.Name == "E2" && x.VirtualHost == "HareDu")
                .FirstOrDefault();
 
            exchange.ShouldNotBeNull();
            exchange.Durable.ShouldBeTrue();
            exchange.Internal.ShouldBeFalse();
            exchange.AutoDelete.ShouldBeFalse();
            exchange.Name.ShouldBe("E2");
            exchange.RoutingType.ShouldBe("direct");
            exchange.VirtualHost.ShouldBe("HareDu");
            exchange.Arguments.ShouldNotBeNull();
            exchange.Arguments.Count.ShouldBe(1);
            exchange.Arguments["alternate-exchange"].ShouldBe("exchange");
        }
    }
}