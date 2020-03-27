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
namespace HareDu.IntegrationTesting.Snapshots
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;
    using Observers;
    using Snapshotting.Model;
    using Snapshotting.Registration;

    [TestFixture]
    public class ConnectionSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu()
                .AddHareDuSnapshot()
                .Build();
        }

        [Test]
        public async Task Test1()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>()
                .RegisterObserver(new DefaultConnectivitySnapshotConsoleLogger())
                .TakeSnapshot();
        }

        [Test]
        public async Task Test2()
        {
            var lens = _container.Resolve<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>()
                .TakeSnapshot();
        }
    }
}