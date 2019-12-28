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
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class SnapshotFactoryTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuSnapshottingModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_return_BrokerConnection_broker_object()
        {
            var snapshot = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerConnection>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<BrokerConnection>();
        }

        [Test]
        public async Task Verify_can_return_BrokerQueues_broker_object()
        {
            var snapshot = _container.Resolve<ISnapshotFactory>()
                .Snapshot<BrokerQueues>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<BrokerQueues>();
        }

        [Test]
        public async Task Verify_can_return_Cluster_broker_object()
        {
            var snapshot = _container.Resolve<ISnapshotFactory>()
                .Snapshot<Cluster>();

            snapshot.ShouldNotBeNull();
            snapshot.ShouldBeAssignableTo<Cluster>();
        }
    }
}