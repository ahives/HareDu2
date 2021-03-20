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
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ConnectionTests :
        HareDuTesting
    {
        [Test]
        public void Test()
        {
            var container = GetContainerBuilder("TestData/ConnectionInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Connection>()
                .GetAll()
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2);
            result.Data[0].Host.ShouldBe("127.0.0.0");
            result.Data[0].Name.ShouldBe("127.0.0.0:79863 -> 127.0.0.0:5672");
            result.Data[0].Node.ShouldBe("rabbit@localhost");
            result.Data[0].State.ShouldBe("running");
            result.Data[0].Type.ShouldBe("network");
            result.Data[0].AuthenticationMechanism.ShouldBe("PLAIN");
            result.Data[0].ConnectedAt.ShouldBe(1572749839566);
            result.Data[0].MaxFrameSizeInBytes.ShouldBe<ulong>(131072);
            result.Data[0].OpenChannelsLimit.ShouldBe<ulong>(2047);
            result.Data[0].PeerHost.ShouldBe("127.0.0.0");
            result.Data[0].PeerPort.ShouldBe(58675);
            result.Data[0].PeerCertificateIssuer.ShouldBeNull();
            result.Data[0].PeerCertificateSubject.ShouldBeNull();
            result.Data[0].TimePeriodPeerCertificateValid.ShouldBeNull();
            result.Data[0].Protocol.ShouldBe("AMQP 0-9-1");
            result.Data[0].Channels.ShouldBe<ulong>(0);
            result.Data[0].PacketsReceived.ShouldBe<ulong>(4);
            result.Data[0].PacketBytesReceived.ShouldBe<ulong>(748);
            result.Data[0].PacketBytesReceivedDetails.ShouldNotBeNull();
            result.Data[0].PacketBytesReceivedDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].PacketsSent.ShouldBe<ulong>(3);
            result.Data[0].PacketBytesSent.ShouldBe<ulong>(542);
            result.Data[0].PacketBytesSentDetails.ShouldNotBeNull();
            result.Data[0].PacketBytesSentDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalReductions.ShouldBe<ulong>(5956);
            result.Data[0].ReductionDetails.ShouldNotBeNull();
            result.Data[0].ReductionDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].SendPending.ShouldBe<ulong>(0);
            result.Data[0].IsSsl.ShouldBeFalse();
            result.Data[0].User.ShouldBe("guest");
            result.Data[0].UserWhoPerformedAction.ShouldBe("guest");
            result.Data[0].ConnectionTimeout.ShouldBe(60);
            result.Data[0].ConnectionClientProperties.ShouldNotBeNull();
            result.Data[0].ConnectionClientProperties?.ClientApi.ShouldBe("MassTransit");
            result.Data[0].ConnectionClientProperties?.ConnectionName.ShouldBe("undefined");
            result.Data[0].ConnectionClientProperties?.Assembly.ShouldBe("HareDu.IntegrationTesting.Publisher");
            result.Data[0].ConnectionClientProperties?.AssemblyVersion.ShouldBe("2.0.0.0");
            result.Data[0].ConnectionClientProperties?.Copyright.ShouldBe("Copyright (c) 2007-2016 Pivotal Software, Inc.");
            result.Data[0].ConnectionClientProperties?.Host.ShouldBe("HareDu");
            result.Data[0].ConnectionClientProperties?.Information.ShouldBe("Licensed under the MPL.  See http://www.rabbitmq.com/");
            result.Data[0].ConnectionClientProperties?.Platform.ShouldBe(".NET");
            result.Data[0].ConnectionClientProperties?.ProcessId.ShouldBe("74446");
            result.Data[0].ConnectionClientProperties?.ProcessName.ShouldBe("dotnet");
            result.Data[0].ConnectionClientProperties?.Product.ShouldBe("RabbitMQ");
            result.Data[0].ConnectionClientProperties?.Version.ShouldBe("5.1.0");
            result.Data[0].ConnectionClientProperties?.Connected.ShouldBe(DateTimeOffset.Parse("Sun, 03 Nov 2019 02:57:19 GMT"));
            result.Data[0].ConnectionClientProperties?.Capabilities.ShouldNotBeNull();
            result.Data[0].ConnectionClientProperties?.Capabilities?.ExchangeBindingEnabled.ShouldBeTrue();
            result.Data[0].ConnectionClientProperties?.Capabilities?.PublisherConfirmsEnabled.ShouldBeTrue();
            result.Data[0].ConnectionClientProperties?.Capabilities?.AuthenticationFailureNotificationEnabled.ShouldBeTrue();
            result.Data[0].ConnectionClientProperties?.Capabilities?.ConnectionBlockedNotificationsEnabled.ShouldBeTrue();
            result.Data[0].ConnectionClientProperties?.Capabilities?.ConsumerCancellationNotificationsEnabled.ShouldBeTrue();
            result.Data[0].ConnectionClientProperties?.Capabilities?.NegativeAcknowledgmentNotificationsEnabled.ShouldBeTrue();
        }
    }
}