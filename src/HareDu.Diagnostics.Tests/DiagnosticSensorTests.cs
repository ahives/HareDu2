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
    using System;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Fakes;
    using Formatting;
    using NUnit.Framework;
    using Observers;
    using Scanning;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticSensorTests :
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
        public void Test1()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot3();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot);

            Console.WriteLine("Diagnostic Report: {0}", report.Identifier.ToString());
            Console.WriteLine("Timestamp: {0}", report.Timestamp.ToString());
            Console.WriteLine();
            
            for (int i = 0; i < report.Results.Count; i++)
            {
                Console.WriteLine("Component Identifier: {0}", report.Results[i].ComponentIdentifier);
                Console.WriteLine("Component Type: {0}", report.Results[i].ComponentType);
                Console.WriteLine("Sensor: {0}", report.Results[i].SensorIdentifier);
                Console.WriteLine("Status: {0}", report.Results[i].Status);
                Console.WriteLine("Data => {0}", report.Results[i].SensorData.ToJsonString());
                
                if (report.Results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine("Reason: {0}", report.Results[i].Reason);
                    Console.WriteLine("Remediation: {0}", report.Results[i].Remediation);
                }
                
                Console.WriteLine();
            }
        }

        [Test]
        public void Verify_connection_channel_limit_reached_sensor_returns_red_status()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot2();
            var report = _container.Resolve<IDiagnosticScanner>()
                .RegisterObserver(new DefaultDiagnosticConsoleLogger())
                .Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Red && x.ComponentType == ComponentType.Connection).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void Verify_unacknowledged_messages_on_channel_not_greater_than_prefetch_count()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot3();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot);
            
            Console.WriteLine(report.ToJsonString());

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Red && x.ComponentType == ComponentType.Channel).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(6, results.Count);
        }

        [Test]
        public void Verify_formatter_works()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot3();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            string report = scanner.Scan(snapshot).Format();
            
            Console.WriteLine(report);
        }
    }
}