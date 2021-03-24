namespace HareDu.Snapshotting.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ClusterSnapshotTests
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
        public async Task Verify_can_return_snapshot()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<ClusterSnapshot>();
            var result = await lens.TakeSnapshot();

            lens.History.Results.Count.ShouldBe(1);
            lens.TakeSnapshot();
            lens.History.Results.Count.ShouldBe(2);

            result.ShouldNotBeNull();
            result.Snapshot.BrokerVersion.ShouldBe("3.7.18");
            result.Snapshot.ClusterName.ShouldBe("fake_cluster");
            result.Snapshot.Nodes[0].ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Memory.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Memory?.AlarmInEffect.ShouldBeTrue();
            result.Snapshot.Nodes[0]?.Memory?.Limit.ShouldBe<ulong>(723746434);
            result.Snapshot.Nodes[0]?.OS.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.OS?.ProcessId.ShouldBe("OS123");
            result.Snapshot.Nodes[0]?.OS?.FileDescriptors.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.OS?.FileDescriptors?.Used.ShouldBe<ulong>(9203797);
            result.Snapshot.Nodes[0]?.OS?.SocketDescriptors.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.OS?.SocketDescriptors?.Used.ShouldBe<ulong>(8298347);
            result.Snapshot.Nodes[0]?.Disk.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.AlarmInEffect.ShouldBeTrue();
            result.Snapshot.Nodes[0]?.Disk?.Capacity.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.Capacity?.Available.ShouldBe<ulong>(7265368234);
            result.Snapshot.Nodes[0]?.Disk?.Limit.ShouldBe<ulong>(8928739432);
            result.Snapshot.Nodes[0]?.AvailableCoresDetected.ShouldBe<ulong>(8);
            result.Snapshot.Nodes[0]?.Disk?.IO.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.IO?.Writes.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.IO?.Writes?.Total.ShouldBe<ulong>(36478608776);
            result.Snapshot.Nodes[0]?.Disk?.IO?.Writes?.Bytes.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.IO?.Writes?.Bytes?.Total.ShouldBe<ulong>(728364283);
            result.Snapshot.Nodes[0]?.Disk?.IO?.Reads.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.IO?.Reads?.Total.ShouldBe<ulong>(892793874982);
            result.Snapshot.Nodes[0]?.Disk?.IO?.Reads?.Bytes.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Disk?.IO?.Reads?.Bytes?.Total.ShouldBe<ulong>(78738764);
            result.Snapshot.Nodes[0]?.Runtime.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Runtime.Processes.ShouldNotBeNull();
            result.Snapshot.Nodes[0]?.Runtime.Processes.Used.ShouldBe<ulong>(9199849);
            result.Snapshot.Nodes[0]?.IsRunning.ShouldBeTrue();
            result.Snapshot.Nodes[0]?.NetworkPartitions.ShouldBe(new List<string>{"partition1", "partition2", "partition3", "partition4"});
        }
    }
}