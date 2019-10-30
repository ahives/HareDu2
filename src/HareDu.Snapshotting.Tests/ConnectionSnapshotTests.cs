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
    using Observers;
    using Registration;

    [TestFixture]
    public class ConnectionSnapshotTests
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
                .Snapshot<BrokerConnection>()
                .Execute();

            BrokerConnectivitySnapshot snapshot = resource.Snapshots.MostRecent().Snapshot;
            
            Assert.AreEqual("3.7.18", snapshot.BrokerVersion);
            Assert.AreEqual("fake_cluster", snapshot.ClusterName);
            Assert.IsNotNull(snapshot.Connections);
            Assert.AreEqual("Connection 1", snapshot.Connections[0].Identifier);
            Assert.AreEqual(ConnectionState.Blocked, snapshot.Connections[0].State);
            Assert.AreEqual(982738, snapshot.Connections[0].OpenChannelsLimit);
            Assert.AreEqual("TestVirtualHost", snapshot.Connections[0].VirtualHost);
            Assert.AreEqual("Node 1", snapshot.Connections[0].NodeIdentifier);
            Assert.IsNotNull(snapshot.Connections[0].NetworkTraffic);
            Assert.IsNotNull(snapshot.Connections[0].NetworkTraffic.Received);
            Assert.AreEqual(68721979894793, snapshot.Connections[0].NetworkTraffic.Received.Total);
            Assert.IsNotNull(snapshot.Connections[0].NetworkTraffic.Sent);
            Assert.AreEqual(871998847, snapshot.Connections[0].NetworkTraffic.Sent.Total);
            Assert.AreEqual(627378937423, snapshot.Connections[0].NetworkTraffic.MaxFrameSize);
            Assert.IsNotNull(snapshot.Connections[0].Channels);
            Assert.IsTrue(snapshot.Connections[0].Channels.Any());
            Assert.AreEqual("Channel 1", snapshot.Connections[0].Channels[0].Identifier);
            Assert.AreEqual(90, snapshot.Connections[0].Channels[0].Consumers);
            Assert.AreEqual(78, snapshot.Connections[0].Channels[0].PrefetchCount);
            Assert.AreEqual(7882003, snapshot.Connections[0].Channels[0].UnacknowledgedMessages);
            Assert.AreEqual(98237843, snapshot.Connections[0].Channels[0].UncommittedAcknowledgements);
            Assert.AreEqual(82930, snapshot.Connections[0].Channels[0].UnconfirmedMessages);
            Assert.AreEqual(383902, snapshot.Connections[0].Channels[0].UncommittedMessages);
//            Assert.AreEqual(, snapshot.Connections[0].Channels[0]);
//            Assert.AreEqual(, snapshot.Connections[0].Channels);
//            Assert.AreEqual(, snapshot.Connections);
        }
    }
}