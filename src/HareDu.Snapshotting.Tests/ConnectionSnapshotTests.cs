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
namespace HareDu.Snapshotting.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using Model;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class ConnectionSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<FakeSnapshotObjectRegistry>()
                .As<ISnapshotObjectRegistry>()
                .SingleInstance();

            builder.RegisterType<FakeBrokerObjectRegistry>()
                .As<IBrokerObjectRegistry>()
                .SingleInstance();
            
            builder.Register(x => new FakeBrokerObjectFactory())
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registration = x.Resolve<ISnapshotObjectRegistry>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    registration.RegisterAll(factory);

                    return new SnapshotFactory(factory, registration.ObjectCache);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerConnection>()
                .Execute();

            BrokerConnectivitySnapshot snapshot = resource.Snapshots.MostRecent().Snapshot;
            
            snapshot.BrokerVersion.ShouldBe("3.7.18");
            snapshot.ClusterName.ShouldBe("fake_cluster");
            snapshot.Connections.ShouldNotBeNull();
            snapshot.Connections[0]?.Identifier.ShouldBe("Connection 1");
            snapshot.Connections[0]?.State.ShouldBe(ConnectionState.Blocked);
            snapshot.Connections[0]?.OpenChannelsLimit.ShouldBe<ulong>(982738);
            snapshot.Connections[0]?.VirtualHost.ShouldBe("TestVirtualHost");
            snapshot.Connections[0]?.NodeIdentifier.ShouldBe("Node 1");
            snapshot.Connections[0]?.NetworkTraffic.ShouldNotBeNull();
            snapshot.Connections[0]?.NetworkTraffic?.Received.ShouldNotBeNull();
            snapshot.Connections[0]?.NetworkTraffic?.Received?.Total.ShouldBe<ulong>(68721979894793);
            snapshot.Connections[0]?.NetworkTraffic?.Sent.ShouldNotBeNull();
            snapshot.Connections[0]?.NetworkTraffic?.Sent?.Total.ShouldBe<ulong>(871998847);
            snapshot.Connections[0]?.NetworkTraffic?.MaxFrameSize.ShouldBe<ulong>(627378937423);
            snapshot.Connections[0]?.Channels.ShouldNotBeNull();
            snapshot.Connections[0]?.Channels.Any().ShouldBeTrue();
            snapshot.Connections[0]?.Channels[0]?.Identifier.ShouldBe("Channel 1");
            snapshot.Connections[0]?.Channels[0]?.Consumers.ShouldBe<ulong>(90);
            snapshot.Connections[0]?.Channels[0]?.PrefetchCount.ShouldBe<uint>(78);
            snapshot.Connections[0]?.Channels[0]?.UnacknowledgedMessages.ShouldBe<ulong>(7882003);
            snapshot.Connections[0]?.Channels[0]?.UncommittedAcknowledgements.ShouldBe<ulong>(98237843);
            snapshot.Connections[0]?.Channels[0]?.UnconfirmedMessages.ShouldBe<ulong>(82930);
            snapshot.Connections[0]?.Channels[0]?.UncommittedMessages.ShouldBe<ulong>(383902);
        }
    }
}