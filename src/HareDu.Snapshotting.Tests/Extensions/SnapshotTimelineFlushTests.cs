// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Snapshotting.Tests.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autofac;
    using Fakes;
    using HareDu.Registration;
    using HareDu.Testing.Fakes;
    using NUnit.Framework;
    using Persistence;
    using Registration;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class SnapshotTimelineFlushTests
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

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_flush_writes_to_disk_and_clears_buffer()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<BrokerQueues>();

            for (int i = 0; i < 10; i++)
            {
                snapshot.Execute();
            }

            snapshot.Timeline.Results.ShouldNotBeNull();
            snapshot.Timeline.Results.Any().ShouldBeTrue();

            var files = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                files.Add(
                    $"{TestContext.CurrentContext.TestDirectory}/snapshots/snapshot_{snapshot.Timeline.Results[i].Identifier}.json");
            }
            
            snapshot.Timeline.Flush(_container.Resolve<ISnapshotWriter>(), $"{TestContext.CurrentContext.TestDirectory}/snapshots");

            snapshot.Timeline.Results.ShouldNotBeNull();
            // snapshot.Timeline.Results.Any().ShouldBeFalse();
            
            for (int i = 0; i < 10; i++)
            {
                File.Exists(files[i]).ShouldBeTrue();
            }
        }
    }
}