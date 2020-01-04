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
namespace HareDu.Snapshotting.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Extensions;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using Model;
    using NUnit.Framework;
    using Registration;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class ConnectionSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<FakeSnapshotObjectRegistrar>()
                .As<ISnapshotObjectRegistrar>()
                .SingleInstance();

            builder.RegisterType<FakeBrokerObjectRegistrar>()
                .As<IBrokerObjectRegistrar>()
                .SingleInstance();
            
            builder.Register(x => new FakeBrokerObjectFactory())
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = x.Resolve<ISnapshotObjectRegistrar>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    registrar.RegisterAll();

                    return new SnapshotFactory(factory, registrar);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var snapshot = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerConnectivity>()
                .Execute();

            var result = snapshot.Timeline.MostRecent();
            
            result.ShouldNotBeNull();
            result.Snapshot.ShouldNotBeNull();
            result.Snapshot.BrokerVersion.ShouldBe("3.7.18");
            result.Snapshot.ClusterName.ShouldBe("fake_cluster");
            result.Snapshot.Connections.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.Identifier.ShouldBe("Connection 1");
            result.Snapshot.Connections[0]?.State.ShouldBe(ConnectionState.Blocked);
            result.Snapshot.Connections[0]?.OpenChannelsLimit.ShouldBe<ulong>(982738);
            result.Snapshot.Connections[0]?.VirtualHost.ShouldBe("TestVirtualHost");
            result.Snapshot.Connections[0]?.NodeIdentifier.ShouldBe("Node 1");
            result.Snapshot.Connections[0]?.NetworkTraffic.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.NetworkTraffic?.Received.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.NetworkTraffic?.Received?.Total.ShouldBe<ulong>(68721979894793);
            result.Snapshot.Connections[0]?.NetworkTraffic?.Sent.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.NetworkTraffic?.Sent?.Total.ShouldBe<ulong>(871998847);
            result.Snapshot.Connections[0]?.NetworkTraffic?.MaxFrameSize.ShouldBe<ulong>(627378937423);
            result.Snapshot.Connections[0]?.Channels.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.Channels.Any().ShouldBeTrue();
            result.Snapshot.Connections[0]?.Channels[0]?.Identifier.ShouldBe("Channel 1");
            result.Snapshot.Connections[0]?.Channels[0]?.Consumers.ShouldBe<ulong>(90);
            result.Snapshot.Connections[0]?.Channels[0]?.PrefetchCount.ShouldBe<uint>(78);
            result.Snapshot.Connections[0]?.Channels[0]?.UnacknowledgedMessages.ShouldBe<ulong>(7882003);
            result.Snapshot.Connections[0]?.Channels[0]?.UncommittedAcknowledgements.ShouldBe<ulong>(98237843);
            result.Snapshot.Connections[0]?.Channels[0]?.UnconfirmedMessages.ShouldBe<ulong>(82930);
            result.Snapshot.Connections[0]?.Channels[0]?.UncommittedMessages.ShouldBe<ulong>(383902);
        }
    }
}