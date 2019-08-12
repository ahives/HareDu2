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
    using NUnit.Framework;
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
        public void Test()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot2();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot);
            
            for (int i = 0; i < report.Results.Count; i++)
            {
                Console.WriteLine("Diagnostic => Sensor: {0}, Status: {1} [Info => {2} \"{3}\"]",
                    report.Results[i].SensorIdentifier,
                    report.Results[i].Status,
                    report.Results[i].ComponentType,
                    report.Results[i].ComponentIdentifier);
                
                Console.WriteLine("Sensor Data => {0}", report.Results[i].SensorData.ToJsonString());
                
                if (report.Results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine("\tReason: {0}", report.Results[i].Reason);
                    Console.WriteLine("\tRemediation: {0}", report.Results[i].Remediation);
                }
                
                Console.WriteLine();
            }
        }

        [Test]
        public void Verify_connection_channel_limit_reached_sensor_returns_red_status()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot2();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot);

            var results = report.Results.Where(x => x.Status == DiagnosticStatus.Red && x.ComponentType == ComponentType.Connection).ToList();
            
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(1, results.Count);
        }
    }
}