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
            
            Assert.IsFalse(result.HasFaulted);
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(2, result.Data.Count);
            Assert.IsNotNull(result.Data[0].ChannelDetails);
            Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672 (1)", result.Data[0].ChannelDetails?.Name);
            Assert.AreEqual("rabbit@localhost", result.Data[0].ChannelDetails?.Node);
            Assert.AreEqual(1, result.Data[0].ChannelDetails?.Number);
            Assert.AreEqual("127.0.0.0:61113 -> 127.0.0.0:5672", result.Data[0].ChannelDetails?.ConnectionName);
            Assert.AreEqual("127.0.0.0", result.Data[0].ChannelDetails?.PeerHost);
            Assert.AreEqual(99883, result.Data[0].ChannelDetails?.PeerPort);
            Assert.AreEqual("guest", result.Data[0].ChannelDetails?.User);
            Assert.IsNotNull(result.Data[0].QueueConsumerDetails);
            Assert.AreEqual("fake_queue", result.Data[0].QueueConsumerDetails?.Name);
            Assert.AreEqual("TestVirtualHost", result.Data[0].QueueConsumerDetails?.VirtualHost);
            Assert.AreEqual("amq.ctag-fOtZo9ajuHDYQQ5hzrgasA", result.Data[0].ConsumerTag);
            Assert.AreEqual(0, result.Data[0].PreFetchCount);
            Assert.IsFalse(result.Data[0].Exclusive);
            Assert.IsTrue(result.Data[0].AcknowledgementRequired);
        }
    }
}