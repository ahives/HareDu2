namespace HareDu.Snapshotting.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using HareDu.Model;
    using Model;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Registration;

    [TestFixture]
    public class ConnectionSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.Register(x => new FakeBrokerObjectFactory())
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.RegisterType<SnapshotFactory>()
                .As<ISnapshotFactory>()
                .SingleInstance();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            var result = await lens.TakeSnapshot();
            
            result.ShouldNotBeNull();
            result.Snapshot.ShouldNotBeNull();
            result.Snapshot.BrokerVersion.ShouldBe("3.7.18");
            result.Snapshot.ClusterName.ShouldBe("fake_cluster");
            result.Snapshot.Connections.ShouldNotBeNull();
            result.Snapshot.Connections[0]?.Identifier.ShouldBe("Connection 1");
            result.Snapshot.Connections[0]?.State.ShouldBe(BrokerConnectionState.Blocked);
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