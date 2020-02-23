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
namespace HareDu.Snapshotting.Tests.Registration
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Registration;

    [TestFixture]
    public class SnapshotFactoryTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuSnapshotModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_return_BrokerConnection_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<BrokerConnectivity>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<BrokerConnectivity>();
        }

        [Test]
        public async Task Verify_can_return_BrokerQueues_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<BrokerQueues>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<BrokerQueues>();
        }

        [Test]
        public async Task Verify_can_return_Cluster_snapshot()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<Cluster>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<Cluster>();
        }

        [Test]
        public async Task Verify_can_return_new_snapshots()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<FakeBrokerSnapshot1>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<FakeBrokerSnapshot1>();
        }

        [Test]
        public void Verify_snapshot_not_implemented_does_not_throw()
        {
            var factory = _container.Resolve<ISnapshotFactory>();
            var snapshot = factory.Snapshot<FakeBrokerSnapshot2>();

            snapshot.ShouldBeNull();
            snapshot.ShouldBeAssignableTo<FakeBrokerSnapshot2>();
        }
    }
}