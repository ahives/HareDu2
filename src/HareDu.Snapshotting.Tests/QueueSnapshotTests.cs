namespace HareDu.Snapshotting.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Persistence;
    using Registration;
    using Shouldly;
    using Snapshotting.Extensions;
    using Snapshotting.Registration;

    [TestFixture]
    public class QueueSnapshotTests
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

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();
            
            _container = builder.Build();
        }
        
        [Test]
        public async Task Test()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerQueuesSnapshot>();
            
            var result = await lens.TakeSnapshot();

            result.Snapshot.ClusterName.ShouldBe("fake_cluster");
            result.Snapshot.Queues.ShouldNotBeNull();
            result.Snapshot.Queues[0].Memory.ShouldNotBeNull();
            result.Snapshot.Queues[0].Memory.PagedOut.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Acknowledged.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Aggregate.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Delivered.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Gets.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Incoming.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Ready.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Redelivered.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.Unacknowledged.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.DeliveredGets.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.DeliveredWithoutAck.ShouldNotBeNull();
            result.Snapshot.Queues[0].Messages.GetsWithoutAck.ShouldNotBeNull();
            result.Snapshot.Churn.ShouldNotBeNull();
            result.Snapshot.Churn.Acknowledged.Total.ShouldNotBeNull();
            result.Snapshot.Churn.Broker.ShouldNotBeNull();
            result.Snapshot.Churn.Delivered.ShouldNotBeNull();
            result.Snapshot.Churn.Gets.ShouldNotBeNull();
            result.Snapshot.Churn.Incoming.ShouldNotBeNull();
            result.Snapshot.Churn.Ready.ShouldNotBeNull();
            result.Snapshot.Churn.Redelivered.ShouldNotBeNull();
            result.Snapshot.Churn.Unacknowledged.ShouldNotBeNull();
            result.Snapshot.Churn.DeliveredGets.ShouldNotBeNull();
            result.Snapshot.Churn.NotRouted.ShouldNotBeNull();
            result.Snapshot.Churn.DeliveredWithoutAck.ShouldNotBeNull();
            result.Snapshot.Churn.GetsWithoutAck.ShouldNotBeNull();
            result.Snapshot.Queues.Any().ShouldBeTrue();
            result.Snapshot.Queues[0].Consumers.ShouldBe<ulong>(773709938);
            result.Snapshot.Queues[0].Memory.Total.ShouldBe<ulong>(92990390);
            result.Snapshot.Queues[0].Memory.PagedOut.Total.ShouldBe<ulong>(90290398);
            result.Snapshot.Queues[0].Memory.PagedOut.Bytes.ShouldBe<ulong>(239939803);
            result.Snapshot.Queues[0].Messages.Acknowledged.Total.ShouldBe<ulong>(92303949398);
            result.Snapshot.Queues[0].Messages.Aggregate.Total.ShouldBe<ulong>(7823668);
            result.Snapshot.Queues[0].Messages.Delivered.Total.ShouldBe<ulong>(238847970);
            result.Snapshot.Queues[0].Messages.Gets.Total.ShouldBe<ulong>(82938820903);
            result.Snapshot.Queues[0].Messages.Incoming.Total.ShouldBe<ulong>(763928923);
            result.Snapshot.Queues[0].Messages.Ready.Total.ShouldBe<ulong>(9293093);
            result.Snapshot.Queues[0].Messages.Redelivered.Total.ShouldBe<ulong>(488983002934);
            result.Snapshot.Queues[0].Messages.Unacknowledged.Total.ShouldBe<ulong>(7273020);
            result.Snapshot.Queues[0].Messages.DeliveredGets.Total.ShouldBe<ulong>(50092830929);
            result.Snapshot.Queues[0].Messages.DeliveredWithoutAck.Total.ShouldBe<ulong>(48898693232);
            result.Snapshot.Queues[0].Messages.GetsWithoutAck.Total.ShouldBe<ulong>(23997979383);
            result.Snapshot.Churn.Acknowledged.Total.ShouldBe<ulong>(7287736);
            result.Snapshot.Churn.Broker.Total.ShouldBe<ulong>(9230748297);
            result.Snapshot.Churn.Broker.Rate.ShouldBe(80.3M);
            result.Snapshot.Churn.Delivered.Total.ShouldBe<ulong>(7234);
            result.Snapshot.Churn.Delivered.Rate.ShouldBe(84);
            result.Snapshot.Churn.Gets.Total.ShouldBe<ulong>(723);
            result.Snapshot.Churn.Gets.Rate.ShouldBe(324);
            result.Snapshot.Churn.Incoming.Total.ShouldBe<ulong>(1234);
            result.Snapshot.Churn.Incoming.Rate.ShouldBe(7);
            result.Snapshot.Churn.Ready.Total.ShouldBe<ulong>(82937489379);
            result.Snapshot.Churn.Ready.Rate.ShouldBe(34.4M);
            result.Snapshot.Churn.Redelivered.Total.ShouldBe<ulong>(838);
            result.Snapshot.Churn.Redelivered.Rate.ShouldBe(89);
            result.Snapshot.Churn.Unacknowledged.Total.ShouldBe<ulong>(892387397238);
            result.Snapshot.Churn.Unacknowledged.Rate.ShouldBe(73.3M);
            result.Snapshot.Churn.DeliveredGets.Total.ShouldBe<ulong>(78263767);
            result.Snapshot.Churn.DeliveredGets.Rate.ShouldBe(738);
            result.Snapshot.Churn.NotRouted.Total.ShouldBe<ulong>(737);
            result.Snapshot.Churn.NotRouted.Rate.ShouldBe(48);
            result.Snapshot.Churn.DeliveredWithoutAck.Total.ShouldBe<ulong>(8723);
            result.Snapshot.Churn.DeliveredWithoutAck.Rate.ShouldBe(56);
            result.Snapshot.Churn.GetsWithoutAck.Total.ShouldBe<ulong>(373);
            result.Snapshot.Churn.GetsWithoutAck.Rate.ShouldBe(84);
        }
    }
}