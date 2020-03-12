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
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_vhosts()
        {
            var container = GetContainerBuilder("TestData/VirtualHostInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .GetResult();
            
            result.HasData.ShouldBeTrue();
            result.Data.Count.ShouldBe(3);
            result.HasFaulted.ShouldBeFalse();
            result.Data[2].Name.ShouldBe("TestVirtualHost");
            result.Data[2].PacketBytesReceived.ShouldBe<ulong>(301363575);
            result.Data[2].RateOfPacketBytesReceived.ShouldNotBeNull();
            result.Data[2].RateOfPacketBytesReceived?.Rate.ShouldBe(0.0M);
            result.Data[2].PacketBytesSent.ShouldBe<ulong>(368933935);
            result.Data[2].RateOfPacketBytesSent.ShouldNotBeNull();
            result.Data[2].RateOfPacketBytesSent?.Rate.ShouldBe(0.0M);
            result.Data[2].TotalMessages.ShouldBe<ulong>(0);
            result.Data[2].RateOfMessages.ShouldNotBeNull();
            result.Data[2].RateOfMessages?.Rate.ShouldBe(0.0M);
            result.Data[2].ReadyMessages.ShouldBe<ulong>(0);
            result.Data[2].RateOfReadyMessages.ShouldNotBeNull();
            result.Data[2].RateOfReadyMessages?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats.ShouldNotBeNull();
            result.Data[2].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesConfirmedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesPublishedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.UnroutableMessagesDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesAcknowledgedDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageDeliveryDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageDeliveryGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessageGetsWithoutAckDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.MessagesRedeliveredDetails.ShouldNotBeNull();
            result.Data[2].MessageStats?.TotalMessagesConfirmed.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesConfirmedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesPublished.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesPublishedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalUnroutableMessages.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.UnroutableMessagesDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesAcknowledged.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessagesAcknowledgedDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesDelivered.ShouldBe<ulong>(300000);
            result.Data[2].MessageStats?.MessageDeliveryDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageDeliveryGets.ShouldBe<ulong>(300003);
            result.Data[2].MessageStats?.MessageDeliveryGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageDeliveredWithoutAck.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageGets.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessageGetDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessageGetsWithoutAck.ShouldBe<ulong>(0);
            result.Data[2].MessageStats?.MessageGetsWithoutAckDetails?.Rate.ShouldBe(0.0M);
            result.Data[2].MessageStats?.TotalMessagesRedelivered.ShouldBe<ulong>(3);
            result.Data[2].MessageStats?.MessagesRedeliveredDetails?.Rate.ShouldBe(0.0M);
        }

        [Test]
        public void Verify_Create_works()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu7");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                })
                .GetResult();
            
            result.DebugInfo.ShouldNotBeNull();
            
            VirtualHostDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostDefinition>();

            definition.Tracing.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_delete()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"))
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost(string.Empty))
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_2()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => {})
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_can_start_vhost()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("FakeVirtualHost", x => x.On("FakeNode"))
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_start_vhost_1()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => x.On("FakeNode"))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_start_vhost_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("FakeVirtualHost", x => x.On(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_start_vhost_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => x.On(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_start_vhost_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup(string.Empty, x => {})
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}