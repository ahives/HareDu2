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
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionTests :
        HareDuTesting
    {
        [Test]
        public async Task Test()
        {
            var container = GetContainerBuilder("TestData/ConnectionInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Connection>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual("127.0.0.0", result.Data[0].Host);
            Assert.AreEqual("127.0.0.0:79863 -> 127.0.0.0:5672", result.Data[0].Name);
            Assert.AreEqual("rabbit@localhost", result.Data[0].Node);
            Assert.AreEqual("running", result.Data[0].State);
            Assert.AreEqual("network", result.Data[0].Type);
            Assert.AreEqual("PLAIN", result.Data[0].AuthenticationMechanism);
            Assert.AreEqual(1572749839566, result.Data[0].ConnectedAt);
            Assert.AreEqual(131072, result.Data[0].MaxFrameSizeInBytes);
            Assert.AreEqual(2047, result.Data[0].OpenChannelsLimit);
            Assert.AreEqual("127.0.0.0", result.Data[0].PeerHost);
            Assert.AreEqual(58675, result.Data[0].PeerPort);
            Assert.IsNull(result.Data[0].PeerCertificateIssuer);
            Assert.IsNull(result.Data[0].PeerCertificateSubject);
            Assert.IsNull(result.Data[0].TimePeriodPeerCertificateValid);
            Assert.AreEqual("AMQP 0-9-1", result.Data[0].Protocol);
            Assert.AreEqual(0, result.Data[0].Channels);
            Assert.AreEqual(4, result.Data[0].PacketsReceived);
            Assert.AreEqual(748, result.Data[0].PacketBytesReceived);
            Assert.IsNotNull(result.Data[0].RateOfPacketBytesReceived);
            Assert.AreEqual(0.0, result.Data[0].RateOfPacketBytesReceived?.Rate);
            Assert.AreEqual(3, result.Data[0].PacketsSent);
            Assert.AreEqual(542, result.Data[0].PacketBytesSent);
            Assert.IsNotNull(result.Data[0].RateOfPacketBytesSent);
            Assert.AreEqual(0.0, result.Data[0].RateOfPacketBytesSent?.Rate);
            Assert.AreEqual(5956, result.Data[0].TotalReductions);
            Assert.IsNotNull(result.Data[0].RateOfReduction);
            Assert.AreEqual(0.0, result.Data[0].RateOfReduction?.Rate);
            Assert.AreEqual(0, result.Data[0].SendPending);
            Assert.IsFalse(result.Data[0].IsSsl);
            Assert.AreEqual("guest", result.Data[0].User);
            Assert.AreEqual("guest", result.Data[0].UserWhoPerformedAction);
            Assert.AreEqual(60, result.Data[0].ConnectionTimeout);
            Assert.IsNotNull(result.Data[0].ClientProperties);
            Assert.AreEqual("MassTransit", result.Data[0].ClientProperties?.ClientApi);
            Assert.AreEqual("undefined", result.Data[0].ClientProperties?.ConnectionName);
            Assert.AreEqual("HareDu.IntegrationTesting.Publisher", result.Data[0].ClientProperties?.Assembly);
            Assert.AreEqual("2.0.0.0", result.Data[0].ClientProperties?.AssemblyVersion);
            Assert.AreEqual("Copyright (c) 2007-2016 Pivotal Software, Inc.", result.Data[0].ClientProperties?.Copyright);
            Assert.AreEqual("HareDu", result.Data[0].ClientProperties?.Host);
            Assert.AreEqual("Licensed under the MPL.  See http://www.rabbitmq.com/", result.Data[0].ClientProperties?.Information);
            Assert.AreEqual(".NET", result.Data[0].ClientProperties?.Platform);
            Assert.AreEqual("74446", result.Data[0].ClientProperties?.ProcessId);
            Assert.AreEqual("dotnet", result.Data[0].ClientProperties?.ProcessName);
            Assert.AreEqual("RabbitMQ", result.Data[0].ClientProperties?.Product);
            Assert.AreEqual("5.1.0", result.Data[0].ClientProperties?.Version);
            Assert.AreEqual(DateTimeOffset.Parse("Sun, 03 Nov 2019 02:57:19 GMT"), result.Data[0].ClientProperties?.Connected);
            Assert.IsNotNull(result.Data[0].ClientProperties?.Capabilities);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.ExchangeBindingEnabled);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.PublisherConfirmsEnabled);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.AuthenticationFailureNotificationEnabled);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.ConnectionBlockedNotificationsEnabled);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.ConsumerCancellationNotificationsEnabled);
            Assert.IsTrue(result.Data[0].ClientProperties?.Capabilities?.NegativeAcknowledgmentNotificationsEnabled);
        }
    }
}