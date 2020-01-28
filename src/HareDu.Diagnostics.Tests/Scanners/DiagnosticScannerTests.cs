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
    using System;
    using System.Collections.Generic;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Fakes;
    using NUnit.Framework;
    using Scanning;
    using Shouldly;
    using Snapshotting;
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
        public void Verify_can_select_BrokerConnectivityDiagnostic()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            var scanner = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);
            
            scanner.ScannerIdentifier.ShouldBe(typeof(BrokerConnectivityDiagnostic).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_ClusterDiagnostic()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            var scanner = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);
            
            scanner.ScannerIdentifier.ShouldBe(typeof(ClusterDiagnostic).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_BrokerQueuesDiagnostic()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            var scanner = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);
            
            scanner.ScannerIdentifier.ShouldBe(typeof(BrokerQueuesDiagnostic).GetIdentifier());
        }

        [Test]
        public void Verify_does_not_throw_when_scanner_not_found()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            IComponentDiagnosticFactory factory = new FakeDiagnosticFactory();
            IDiagnosticScanner scanner = new DiagnosticScanner(factory);

            var report = scanner.Scan(snapshot);
            
            report.ScannerIdentifier.ShouldBe(typeof(NoOpDiagnostic<EmptySnapshot>).GetIdentifier());
            report.ShouldBe(DiagnosticCache.EmptyScannerResult);
        }

        
        class FakeDiagnosticFactory :
            IComponentDiagnosticFactory
        {
            public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
                where T : Snapshot
            {
                diagnostic = new NoOpDiagnostic<T>();
                return false;
            }

            public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticProbeContext>> observers)
            {
                throw new NotImplementedException();
            }

            public void RegisterObserver(IObserver<DiagnosticProbeContext> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}