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
    using Model;
    using NUnit.Framework;
    using Observers;
    using Registration;

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
        public async Task Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<RmqCluster>()
//                .RegisterObserver(new DefaultClusterSnapshotConsoleLogger())
                .Execute();

            ClusterSnapshot snapshot = resource.Snapshots.MostRecent().Snapshot;
            
            Assert.AreEqual("3.7.18", snapshot.BrokerVersion);
            Assert.AreEqual("fake_cluster", snapshot.ClusterName);
            Assert.AreEqual(new List<string>{"partition1", "partition2", "partition3", "partition4"}, snapshot.Nodes[0].NetworkPartitions);
            Assert.IsNotNull(snapshot.Nodes[0].Memory);
            Assert.IsTrue(snapshot.Nodes[0].Memory.AlarmInEffect);
            Assert.AreEqual(723746434, snapshot.Nodes[0].Memory.Limit);
            Assert.IsNotNull(snapshot.Nodes[0].OS);
            Assert.AreEqual("OS123", snapshot.Nodes[0].OS.ProcessId);
            Assert.IsNotNull(snapshot.Nodes[0].OS.FileDescriptors);
            Assert.AreEqual(9203797, snapshot.Nodes[0].OS.FileDescriptors.Used);
            Assert.IsNotNull(snapshot.Nodes[0].OS.SocketDescriptors);
            Assert.AreEqual(8298347, snapshot.Nodes[0].OS.SocketDescriptors.Used);
            Assert.IsNotNull(snapshot.Nodes[0].Disk);
            Assert.IsTrue(snapshot.Nodes[0].Disk.AlarmInEffect);
            Assert.IsNotNull(snapshot.Nodes[0].Disk.Capacity);
            Assert.AreEqual(7265368234, snapshot.Nodes[0].Disk.Capacity.Available);
            Assert.AreEqual(8928739432, snapshot.Nodes[0].Disk.Limit);
            Assert.AreEqual(8, snapshot.Nodes[0].AvailableCoresDetected);
            Assert.IsNotNull(snapshot.Nodes[0].Disk.IO);
            Assert.IsNotNull(snapshot.Nodes[0].Disk.IO.Writes);
            Assert.AreEqual(36478608776, snapshot.Nodes[0].Disk.IO.Writes.Total);
            Assert.AreEqual(728364283, snapshot.Nodes[0].Disk.IO.Writes.Bytes.Total);
//            Assert.AreEqual(, snapshot.Nodes[0].IO.Writes);
            Assert.IsNotNull(snapshot.Nodes[0].Disk.IO.Reads);
            Assert.AreEqual(892793874982, snapshot.Nodes[0].Disk.IO.Reads.Total);
            Assert.AreEqual(78738764, snapshot.Nodes[0].Disk.IO.Reads.Bytes.Total);
            Assert.IsNotNull(snapshot.Nodes[0].Runtime);
            Assert.IsNotNull(snapshot.Nodes[0].Runtime.Processes);
            Assert.AreEqual(9199849, snapshot.Nodes[0].Runtime.Processes.Used);
            Assert.IsTrue(snapshot.Nodes[0].IsRunning);
//            Assert.AreEqual(, snapshot.Nodes[0]);
        }
    }
}