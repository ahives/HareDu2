namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NewConnectionTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_get_all_connections1()
        {
            var services = GetContainerBuilder("TestData/ConnectionInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Connection>()
                .GetAll();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("127.0.0.0", result.Data[0].Host);
                Assert.AreEqual("127.0.0.0:79863 -> 127.0.0.0:5672", result.Data[0].Name);
                Assert.AreEqual("rabbit@localhost", result.Data[0].Node);
                Assert.AreEqual(BrokerConnectionState.Running, result.Data[0].State);
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
                Assert.IsNotNull(result.Data[0].PacketBytesReceivedDetails);
                Assert.AreEqual(0.0M, result.Data[0].PacketBytesReceivedDetails?.Value);
                Assert.AreEqual(3, result.Data[0].PacketsSent);
                Assert.AreEqual(542, result.Data[0].PacketBytesSent);
                Assert.IsNotNull(result.Data[0].PacketBytesSentDetails);
                Assert.AreEqual(0.0M, result.Data[0].PacketBytesSentDetails?.Value);
                Assert.AreEqual(5956, result.Data[0].TotalReductions);
                Assert.IsNotNull(result.Data[0].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[0].ReductionDetails?.Value);
                Assert.AreEqual(0, result.Data[0].SendPending);
                Assert.IsFalse(result.Data[0].IsSsl);
                Assert.AreEqual("guest", result.Data[0].User);
                Assert.AreEqual("guest", result.Data[0].UserWhoPerformedAction);
                Assert.AreEqual(60, result.Data[0].ConnectionTimeout);
                Assert.IsNotNull(result.Data[0].ConnectionClientProperties);
                Assert.AreEqual("MassTransit", result.Data[0].ConnectionClientProperties?.ClientApi);
                Assert.AreEqual("undefined", result.Data[0].ConnectionClientProperties?.ConnectionName);
                Assert.AreEqual("HareDu.IntegrationTesting.Publisher", result.Data[0].ConnectionClientProperties?.Assembly);
                Assert.AreEqual("2.0.0.0", result.Data[0].ConnectionClientProperties?.AssemblyVersion);
                Assert.AreEqual("Copyright (c) 2007-2016 Pivotal Software, Inc.", result.Data[0].ConnectionClientProperties?.Copyright);
                Assert.AreEqual("HareDu", result.Data[0].ConnectionClientProperties?.Host);
                Assert.AreEqual("Licensed under the MPL.  See http://www.rabbitmq.com/", result.Data[0].ConnectionClientProperties?.Information);
                Assert.AreEqual(".NET", result.Data[0].ConnectionClientProperties?.Platform);
                Assert.AreEqual("74446", result.Data[0].ConnectionClientProperties?.ProcessId);
                Assert.AreEqual("dotnet", result.Data[0].ConnectionClientProperties?.ProcessName);
                Assert.AreEqual("RabbitMQ", result.Data[0].ConnectionClientProperties?.Product);
                Assert.AreEqual("5.1.0", result.Data[0].ConnectionClientProperties?.Version);
                Assert.AreEqual(DateTimeOffset.Parse("Sun, 03 Nov 2019 02:57:19 GMT"), result.Data[0].ConnectionClientProperties?.Connected);
                Assert.IsNotNull(result.Data[0].ConnectionClientProperties?.Capabilities);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ExchangeBindingEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.PublisherConfirmsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.AuthenticationFailureNotificationEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ConnectionBlockedNotificationsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ConsumerCancellationNotificationsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.NegativeAcknowledgmentNotificationsEnabled);
            });
        }

        [Test]
        public async Task Verify_can_get_all_connections2()
        {
            var services = GetContainerBuilder("TestData/ConnectionInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllConnections();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("127.0.0.0", result.Data[0].Host);
                Assert.AreEqual("127.0.0.0:79863 -> 127.0.0.0:5672", result.Data[0].Name);
                Assert.AreEqual("rabbit@localhost", result.Data[0].Node);
                Assert.AreEqual(BrokerConnectionState.Running, result.Data[0].State);
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
                Assert.IsNotNull(result.Data[0].PacketBytesReceivedDetails);
                Assert.AreEqual(0.0M, result.Data[0].PacketBytesReceivedDetails?.Value);
                Assert.AreEqual(3, result.Data[0].PacketsSent);
                Assert.AreEqual(542, result.Data[0].PacketBytesSent);
                Assert.IsNotNull(result.Data[0].PacketBytesSentDetails);
                Assert.AreEqual(0.0M, result.Data[0].PacketBytesSentDetails?.Value);
                Assert.AreEqual(5956, result.Data[0].TotalReductions);
                Assert.IsNotNull(result.Data[0].ReductionDetails);
                Assert.AreEqual(0.0M, result.Data[0].ReductionDetails?.Value);
                Assert.AreEqual(0, result.Data[0].SendPending);
                Assert.IsFalse(result.Data[0].IsSsl);
                Assert.AreEqual("guest", result.Data[0].User);
                Assert.AreEqual("guest", result.Data[0].UserWhoPerformedAction);
                Assert.AreEqual(60, result.Data[0].ConnectionTimeout);
                Assert.IsNotNull(result.Data[0].ConnectionClientProperties);
                Assert.AreEqual("MassTransit", result.Data[0].ConnectionClientProperties?.ClientApi);
                Assert.AreEqual("undefined", result.Data[0].ConnectionClientProperties?.ConnectionName);
                Assert.AreEqual("HareDu.IntegrationTesting.Publisher", result.Data[0].ConnectionClientProperties?.Assembly);
                Assert.AreEqual("2.0.0.0", result.Data[0].ConnectionClientProperties?.AssemblyVersion);
                Assert.AreEqual("Copyright (c) 2007-2016 Pivotal Software, Inc.", result.Data[0].ConnectionClientProperties?.Copyright);
                Assert.AreEqual("HareDu", result.Data[0].ConnectionClientProperties?.Host);
                Assert.AreEqual("Licensed under the MPL.  See http://www.rabbitmq.com/", result.Data[0].ConnectionClientProperties?.Information);
                Assert.AreEqual(".NET", result.Data[0].ConnectionClientProperties?.Platform);
                Assert.AreEqual("74446", result.Data[0].ConnectionClientProperties?.ProcessId);
                Assert.AreEqual("dotnet", result.Data[0].ConnectionClientProperties?.ProcessName);
                Assert.AreEqual("RabbitMQ", result.Data[0].ConnectionClientProperties?.Product);
                Assert.AreEqual("5.1.0", result.Data[0].ConnectionClientProperties?.Version);
                Assert.AreEqual(DateTimeOffset.Parse("Sun, 03 Nov 2019 02:57:19 GMT"), result.Data[0].ConnectionClientProperties?.Connected);
                Assert.IsNotNull(result.Data[0].ConnectionClientProperties?.Capabilities);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ExchangeBindingEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.PublisherConfirmsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.AuthenticationFailureNotificationEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ConnectionBlockedNotificationsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.ConsumerCancellationNotificationsEnabled);
                Assert.IsTrue(result.Data[0].ConnectionClientProperties?.Capabilities?.NegativeAcknowledgmentNotificationsEnabled);
            });
        }
    }
}