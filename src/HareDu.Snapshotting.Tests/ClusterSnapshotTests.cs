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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using Model;
    using NUnit.Framework;
    using Observers;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class ClusterSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FakeSnapshotRegistration>()
                .As<ISnapshotRegistration>()
                .SingleInstance();

            builder.RegisterType<FakeBrokerObjectRegistration>()
                .As<IBrokerObjectRegistration>()
                .SingleInstance();
            
            builder.Register(x => new FakeBrokerObjectFactory())
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registration = x.Resolve<ISnapshotRegistration>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    registration.RegisterAll(factory);

                    return new SnapshotFactory(factory, registration.Cache);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_return_snapshot()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<RmqCluster>()
                .Execute();

            ClusterSnapshot snapshot = resource.Snapshots.MostRecent().Snapshot;
            
            snapshot.BrokerVersion.ShouldBe("3.7.18");
            snapshot.ClusterName.ShouldBe("fake_cluster");
            snapshot.Nodes[0].ShouldNotBeNull();
            snapshot.Nodes[0]?.Memory.ShouldNotBeNull();
            snapshot.Nodes[0]?.Memory?.AlarmInEffect.ShouldBeTrue();
            snapshot.Nodes[0]?.Memory?.Limit.ShouldBe<ulong>(723746434);
            snapshot.Nodes[0]?.OS.ShouldNotBeNull();
            snapshot.Nodes[0]?.OS?.ProcessId.ShouldBe("OS123");
            snapshot.Nodes[0]?.OS?.FileDescriptors.ShouldNotBeNull();
            snapshot.Nodes[0]?.OS?.FileDescriptors?.Used.ShouldBe<ulong>(9203797);
            snapshot.Nodes[0]?.OS?.SocketDescriptors.ShouldNotBeNull();
            snapshot.Nodes[0]?.OS?.SocketDescriptors?.Used.ShouldBe<ulong>(8298347);
            snapshot.Nodes[0]?.Disk.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.AlarmInEffect.ShouldBeTrue();
            snapshot.Nodes[0]?.Disk?.Capacity.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.Capacity?.Available.ShouldBe<ulong>(7265368234);
            snapshot.Nodes[0]?.Disk?.Limit.ShouldBe<ulong>(8928739432);
            snapshot.Nodes[0]?.AvailableCoresDetected.ShouldBe<ulong>(8);
            snapshot.Nodes[0]?.Disk?.IO.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.IO?.Writes.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.IO?.Writes?.Total.ShouldBe<ulong>(36478608776);
            snapshot.Nodes[0]?.Disk?.IO?.Writes?.Bytes.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.IO?.Writes?.Bytes?.Total.ShouldBe<ulong>(728364283);
            snapshot.Nodes[0]?.Disk?.IO?.Reads.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.IO?.Reads?.Total.ShouldBe<ulong>(892793874982);
            snapshot.Nodes[0]?.Disk?.IO?.Reads?.Bytes.ShouldNotBeNull();
            snapshot.Nodes[0]?.Disk?.IO?.Reads?.Bytes?.Total.ShouldBe<ulong>(78738764);
            snapshot.Nodes[0]?.Runtime.ShouldNotBeNull();
            snapshot.Nodes[0]?.Runtime.Processes.ShouldNotBeNull();
            snapshot.Nodes[0]?.Runtime.Processes.Used.ShouldBe<ulong>(9199849);
            snapshot.Nodes[0]?.IsRunning.ShouldBeTrue();
            snapshot.Nodes[0]?.NetworkPartitions.ShouldBe(new List<string>{"partition1", "partition2", "partition3", "partition4"});
        }
    }
}