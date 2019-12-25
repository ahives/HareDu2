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
namespace HareDu.Tests.BrokerObjects
{
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ConsumerTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/ConsumerInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Consumer>()
                .GetAll();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2);
            result.Data[0].ChannelDetails.ShouldNotBeNull();
            result.Data[0].ChannelDetails?.Name.ShouldBe("127.0.0.0:61113 -> 127.0.0.0:5672 (1)");
            result.Data[0].ChannelDetails?.Node.ShouldBe("rabbit@localhost");
            result.Data[0].ChannelDetails?.Number.ShouldBe(1);
            result.Data[0].ChannelDetails?.ConnectionName.ShouldBe("127.0.0.0:61113 -> 127.0.0.0:5672");
            result.Data[0].ChannelDetails?.PeerHost.ShouldBe("127.0.0.0");
            result.Data[0].ChannelDetails?.PeerPort.ShouldBe(99883);
            result.Data[0].ChannelDetails?.User.ShouldBe("guest");
            result.Data[0].QueueConsumerDetails.ShouldNotBeNull();
            result.Data[0].QueueConsumerDetails?.Name.ShouldBe("fake_queue");
            result.Data[0].QueueConsumerDetails?.VirtualHost.ShouldBe("TestVirtualHost");
            result.Data[0].ConsumerTag.ShouldBe("amq.ctag-fOtZo9ajuHDYQQ5hzrgasA");
            result.Data[0].PreFetchCount.ShouldBe<ulong>(0);
            result.Data[0].Exclusive.ShouldBeFalse();
            result.Data[0].AcknowledgementRequired.ShouldBeTrue();
        }
    }
}