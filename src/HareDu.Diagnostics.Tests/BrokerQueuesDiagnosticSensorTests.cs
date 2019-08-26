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
namespace HareDu.Diagnostics.Tests
{
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Fakes;
    using NUnit.Framework;
    using Observers;
    using Scanning;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerQueuesDiagnosticSensorTests :
        DiagnosticsTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();
        }

        [Test]
        public void Verify_broker_queues_memory_paging_warning_status()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1();
            var report = _container.Resolve<IDiagnosticScanner>()
                .RegisterObserver(new DefaultDiagnosticConsoleLogger())
                .Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Yellow && x.ComponentType == ComponentType.Queue).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
//            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void Verify_queue_depth_growth_detected()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot2();
            var report = _container.Resolve<IDiagnosticScanner>()
                .RegisterObserver(new DefaultDiagnosticConsoleLogger())
                .Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Yellow && x.ComponentType == ComponentType.Queue).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
//            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void Verify_queue_redelivery_()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot3();
            var report = _container.Resolve<IDiagnosticScanner>()
                .RegisterObserver(new DefaultDiagnosticConsoleLogger())
                .Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Yellow && x.ComponentType == ComponentType.Queue).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
//            Assert.AreEqual(1, results.Count);
        }

    }
}