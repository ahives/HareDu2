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
    using Extensions;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using Model;
    using NUnit.Framework;
    using Observers;
    using Registration;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class ClusterSnapshotTests
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
        public async Task Verify_can_return_snapshot()
        {
            var snapshot = _container.Resolve<ISnapshotFactory>()
                .Snapshot<Cluster>()
                .Execute();

            var result = snapshot.Timeline.MostRecent();
            
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