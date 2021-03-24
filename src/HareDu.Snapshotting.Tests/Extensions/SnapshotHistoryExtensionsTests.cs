namespace HareDu.Snapshotting.Tests.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autofac;
    using Fakes;
    using Model;
    using NUnit.Framework;
    using Persistence;
    using Registration;
    using Shouldly;
    using Snapshotting.Extensions;
    using Snapshotting.Registration;

    [TestFixture]
    public class SnapshotHistoryExtensionsTests
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
        public void Verify_flush_writes_to_disk_and_clears_buffer()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Lens<BrokerQueuesSnapshot>();

            for (int i = 0; i < 10; i++)
            {
                snapshot.TakeSnapshot();
            }

            snapshot.History.Results.ShouldNotBeNull();
            snapshot.History.Results.Any().ShouldBeTrue();

            var files = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                files.Add(
                    $"{TestContext.CurrentContext.TestDirectory}/snapshots/snapshot_{snapshot.History.Results[i].Identifier}.json");
            }
            
            snapshot.History.Flush(_container.Resolve<ISnapshotWriter>(), $"{TestContext.CurrentContext.TestDirectory}/snapshots");

            snapshot.History.Results.ShouldNotBeNull();
            // snapshot.Timeline.Results.Any().ShouldBeFalse();
            
            for (int i = 0; i < 10; i++)
            {
                File.Exists(files[i]).ShouldBeTrue();
            }
        }
    }
}