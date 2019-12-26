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
    using Core;
    using Core.Configuration;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using Model;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class QueueSnapshotTests
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
        public void Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerQueues>()
                .Execute();

            BrokerQueuesSnapshot snapshot = resource.Timeline.MostRecent().Snapshot;
            
            snapshot.ClusterName.ShouldBe("fake_cluster");
            snapshot.Queues.ShouldNotBeNull();
            snapshot.Queues[0].Memory.ShouldNotBeNull();
            snapshot.Queues[0].Memory.PagedOut.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Acknowledged.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Aggregate.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Delivered.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Gets.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Incoming.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Ready.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Redelivered.ShouldNotBeNull();
            snapshot.Queues[0].Messages.Unacknowledged.ShouldNotBeNull();
            snapshot.Queues[0].Messages.DeliveredGets.ShouldNotBeNull();
            snapshot.Queues[0].Messages.DeliveredWithoutAck.ShouldNotBeNull();
            snapshot.Queues[0].Messages.GetsWithoutAck.ShouldNotBeNull();
            snapshot.Churn.ShouldNotBeNull();
            snapshot.Churn.Acknowledged.Total.ShouldNotBeNull();
            snapshot.Churn.Broker.ShouldNotBeNull();
            snapshot.Churn.Delivered.ShouldNotBeNull();
            snapshot.Churn.Gets.ShouldNotBeNull();
            snapshot.Churn.Incoming.ShouldNotBeNull();
            snapshot.Churn.Ready.ShouldNotBeNull();
            snapshot.Churn.Redelivered.ShouldNotBeNull();
            snapshot.Churn.Unacknowledged.ShouldNotBeNull();
            snapshot.Churn.DeliveredGets.ShouldNotBeNull();
            snapshot.Churn.NotRouted.ShouldNotBeNull();
            snapshot.Churn.DeliveredWithoutAck.ShouldNotBeNull();
            snapshot.Churn.GetsWithoutAck.ShouldNotBeNull();
            snapshot.Queues.Any().ShouldBeTrue();
            snapshot.Queues[0].Consumers.ShouldBe<ulong>(773709938);
            snapshot.Queues[0].Memory.Total.ShouldBe<ulong>(92990390);
            snapshot.Queues[0].Memory.PagedOut.Total.ShouldBe<ulong>(90290398);
            snapshot.Queues[0].Memory.PagedOut.Bytes.ShouldBe<ulong>(239939803);
            snapshot.Queues[0].Messages.Acknowledged.Total.ShouldBe<ulong>(92303949398);
            snapshot.Queues[0].Messages.Aggregate.Total.ShouldBe<ulong>(7823668);
            snapshot.Queues[0].Messages.Delivered.Total.ShouldBe<ulong>(238847970);
            snapshot.Queues[0].Messages.Gets.Total.ShouldBe<ulong>(82938820903);
            snapshot.Queues[0].Messages.Incoming.Total.ShouldBe<ulong>(763928923);
            snapshot.Queues[0].Messages.Ready.Total.ShouldBe<ulong>(9293093);
            snapshot.Queues[0].Messages.Redelivered.Total.ShouldBe<ulong>(488983002934);
            snapshot.Queues[0].Messages.Unacknowledged.Total.ShouldBe<ulong>(7273020);
            snapshot.Queues[0].Messages.DeliveredGets.Total.ShouldBe<ulong>(50092830929);
            snapshot.Queues[0].Messages.DeliveredWithoutAck.Total.ShouldBe<ulong>(48898693232);
            snapshot.Queues[0].Messages.GetsWithoutAck.Total.ShouldBe<ulong>(23997979383);
            snapshot.Churn.Acknowledged.Total.ShouldBe<ulong>(7287736);
            snapshot.Churn.Broker.Total.ShouldBe<ulong>(9230748297);
            snapshot.Churn.Broker.Rate.ShouldBe(80.3M);
            snapshot.Churn.Delivered.Total.ShouldBe<ulong>(7234);
            snapshot.Churn.Delivered.Rate.ShouldBe(84);
            snapshot.Churn.Gets.Total.ShouldBe<ulong>(723);
            snapshot.Churn.Gets.Rate.ShouldBe(324);
            snapshot.Churn.Incoming.Total.ShouldBe<ulong>(1234);
            snapshot.Churn.Incoming.Rate.ShouldBe(7);
            snapshot.Churn.Ready.Total.ShouldBe<ulong>(82937489379);
            snapshot.Churn.Ready.Rate.ShouldBe(34.4M);
            snapshot.Churn.Redelivered.Total.ShouldBe<ulong>(838);
            snapshot.Churn.Redelivered.Rate.ShouldBe(89);
            snapshot.Churn.Unacknowledged.Total.ShouldBe<ulong>(892387397238);
            snapshot.Churn.Unacknowledged.Rate.ShouldBe(73.3M);
            snapshot.Churn.DeliveredGets.Total.ShouldBe<ulong>(78263767);
            snapshot.Churn.DeliveredGets.Rate.ShouldBe(738);
            snapshot.Churn.NotRouted.Total.ShouldBe<ulong>(737);
            snapshot.Churn.NotRouted.Rate.ShouldBe(48);
            snapshot.Churn.DeliveredWithoutAck.Total.ShouldBe<ulong>(8723);
            snapshot.Churn.DeliveredWithoutAck.Rate.ShouldBe(56);
            snapshot.Churn.GetsWithoutAck.Total.ShouldBe<ulong>(373);
            snapshot.Churn.GetsWithoutAck.Rate.ShouldBe(84);
        }
    }
}