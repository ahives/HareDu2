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
namespace HareDu.Diagnostics.Tests.Scanners
{
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Diagnostics.Registration;
    using Diagnostics.Scanners;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuDiagnosticsModule>();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_can_select_broker_connectivity_scanner()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            var result = _container.Resolve<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(BrokerConnectivityScanner).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_cluster_scanner()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            var result = _container.Resolve<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(ClusterScanner).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_broker_queues_scanner()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            var result = _container.Resolve<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(BrokerQueuesScanner).GetIdentifier());
        }

        [Test]
        public void Verify_does_not_throw_when_scanner_not_found()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            IScannerFactory factory = new FakeScannerFactory();
            IScanner result = new Scanner(factory);

            var report = result.Scan(snapshot);
            
            report.ScannerId.ShouldBe(typeof(NoOpScanner<EmptySnapshot>).GetIdentifier());
            report.ShouldBe(DiagnosticCache.EmptyScannerResult);
        }
    }
}