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
namespace HareDu.Diagnostics.Tests.Scanners
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using AutofacIntegration;
    using Fakes;
    using NUnit.Framework;
    using Scanning;
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
            
            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), scanner.ScannerIdentifier);
        }

        [Test]
        public void Verify_can_select_ClusterDiagnostic()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshotSnapshot1();
            var scanner = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);
            
            Assert.AreEqual(typeof(ClusterDiagnostic).FullName.GenerateIdentifier(), scanner.ScannerIdentifier);
        }

        [Test]
        public void Verify_can_select_BrokerQueuesDiagnostic()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            var scanner = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);
            
            Assert.AreEqual(typeof(BrokerQueuesDiagnostic).FullName.GenerateIdentifier(), scanner.ScannerIdentifier);
        }

        [Test]
        public void Verify_does_not_throw_when_scanner_not_found()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            IComponentDiagnosticFactory factory = new FakeDiagnosticFactory();
            IDiagnosticScanner scanner = new DiagnosticScanner(factory);

            var report = scanner.Scan(snapshot);
            
            Assert.AreEqual(DiagnosticCache.EmptyDiagnosticReport, report);
            Assert.AreEqual(typeof(DoNothingDiagnostic<EmptySnapshot>).FullName.GenerateIdentifier(), report.ScannerIdentifier);
        }

        
        class FakeDiagnosticFactory :
            IComponentDiagnosticFactory
        {
            public bool TryGet<T>(out IComponentDiagnostic<T> diagnostic)
                where T : Snapshot
            {
                diagnostic = new DoNothingDiagnostic<T>();
                return false;
            }

            public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticContext>> observers)
            {
                throw new NotImplementedException();
            }

            public void RegisterObservers(IReadOnlyList<IObserver<DiagnosticSensorContext>> observers)
            {
                throw new NotImplementedException();
            }

            public void RegisterObserver(IObserver<DiagnosticContext> observer)
            {
                throw new NotImplementedException();
            }

            public void RegisterObserver(IObserver<DiagnosticSensorContext> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}