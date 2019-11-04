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

    [TestFixture]
    public class QueueSnapshotTests
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
        public void Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerQueues>()
                .Execute();

            BrokerQueuesSnapshot snapshot = resource.Snapshots.MostRecent().Snapshot;
            
            Assert.AreEqual("fake_cluster", snapshot.ClusterName);
            Assert.IsNotNull(snapshot.Queues);
            Assert.IsTrue(snapshot.Queues.Any());
            Assert.AreEqual(773709938, snapshot.Queues[0].Consumers);
            Assert.IsNotNull(snapshot.Queues[0].Memory);
            Assert.AreEqual(92990390, snapshot.Queues[0].Memory.Total);
            Assert.IsNotNull(snapshot.Queues[0].Memory.PagedOut);
            Assert.AreEqual(90290398, snapshot.Queues[0].Memory.PagedOut.Total);
            Assert.AreEqual(239939803, snapshot.Queues[0].Memory.PagedOut.Bytes);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Acknowledged);
            Assert.AreEqual(92303949398, snapshot.Queues[0].Messages.Acknowledged.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Aggregate);
            Assert.AreEqual(7823668, snapshot.Queues[0].Messages.Aggregate.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Delivered);
            Assert.AreEqual(238847970, snapshot.Queues[0].Messages.Delivered.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Gets);
            Assert.AreEqual(82938820903, snapshot.Queues[0].Messages.Gets.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Incoming);
            Assert.AreEqual(763928923, snapshot.Queues[0].Messages.Incoming.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Ready);
            Assert.AreEqual(9293093, snapshot.Queues[0].Messages.Ready.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Redelivered);
            Assert.AreEqual(488983002934, snapshot.Queues[0].Messages.Redelivered.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.Unacknowledged);
            Assert.AreEqual(7273020, snapshot.Queues[0].Messages.Unacknowledged.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.DeliveredGets);
            Assert.AreEqual(50092830929, snapshot.Queues[0].Messages.DeliveredGets.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.DeliveredWithoutAck);
            Assert.AreEqual(48898693232, snapshot.Queues[0].Messages.DeliveredWithoutAck.Total);
            Assert.IsNotNull(snapshot.Queues[0].Messages.GetsWithoutAck);
            Assert.AreEqual(23997979383, snapshot.Queues[0].Messages.GetsWithoutAck.Total);
            Assert.IsNotNull(snapshot.Churn);
            Assert.IsNotNull(snapshot.Churn.Acknowledged.Total);
            Assert.AreEqual(7287736, snapshot.Churn.Acknowledged.Total);
            Assert.IsNotNull(snapshot.Churn.Broker);
            Assert.AreEqual(9230748297, snapshot.Churn.Broker.Total);
            Assert.AreEqual(80.3M, snapshot.Churn.Broker.Rate);
            Assert.IsNotNull(snapshot.Churn.Delivered);
            Assert.AreEqual(7234, snapshot.Churn.Delivered.Total);
            Assert.AreEqual(84, snapshot.Churn.Delivered.Rate);
            Assert.IsNotNull(snapshot.Churn.Gets);
            Assert.AreEqual(723, snapshot.Churn.Gets.Total);
            Assert.AreEqual(324, snapshot.Churn.Gets.Rate);
            Assert.IsNotNull(snapshot.Churn.Incoming);
            Assert.AreEqual(1234, snapshot.Churn.Incoming.Total);
            Assert.AreEqual(7, snapshot.Churn.Incoming.Rate);
            Assert.IsNotNull(snapshot.Churn.Ready);
            Assert.AreEqual(82937489379, snapshot.Churn.Ready.Total);
            Assert.AreEqual(34.4M, snapshot.Churn.Ready.Rate);
            Assert.IsNotNull(snapshot.Churn.Redelivered);
            Assert.AreEqual(838, snapshot.Churn.Redelivered.Total);
            Assert.AreEqual(89, snapshot.Churn.Redelivered.Rate);
            Assert.IsNotNull(snapshot.Churn.Unacknowledged);
            Assert.AreEqual(892387397238, snapshot.Churn.Unacknowledged.Total);
            Assert.AreEqual(73.3M, snapshot.Churn.Unacknowledged.Rate);
            Assert.IsNotNull(snapshot.Churn.DeliveredGets);
            Assert.AreEqual(78263767, snapshot.Churn.DeliveredGets.Total);
            Assert.AreEqual(738, snapshot.Churn.DeliveredGets.Rate);
            Assert.IsNotNull(snapshot.Churn.NotRouted);
            Assert.AreEqual(737, snapshot.Churn.NotRouted.Total);
            Assert.AreEqual(48, snapshot.Churn.NotRouted.Rate);
            Assert.IsNotNull(snapshot.Churn.DeliveredWithoutAck);
            Assert.AreEqual(8723, snapshot.Churn.DeliveredWithoutAck.Total);
            Assert.AreEqual(56, snapshot.Churn.DeliveredWithoutAck.Rate);
            Assert.IsNotNull(snapshot.Churn.GetsWithoutAck);
            Assert.AreEqual(373, snapshot.Churn.GetsWithoutAck.Total);
            Assert.AreEqual(84, snapshot.Churn.GetsWithoutAck.Rate);
        }
    }
}