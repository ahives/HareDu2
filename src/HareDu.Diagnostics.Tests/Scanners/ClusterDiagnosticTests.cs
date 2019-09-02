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
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Diagnostics.Sensors;
    using Fakes;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterDiagnosticTests
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
        public void Verify_sensors_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshotSnapshot1();
            var report = _container.Resolve<IDiagnosticScanner>()
                .Scan(snapshot);

            Assert.AreEqual(5, report.Results.Count);
            Assert.AreEqual(1, report.Results.Count(x => x.SensorIdentifier == typeof(RuntimeProcessLimitReachedSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Results.Count(x => x.SensorIdentifier == typeof(NetworkThrottlingSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Results.Count(x => x.SensorIdentifier == typeof(NetworkPartitionSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Results.Count(x => x.SensorIdentifier == typeof(MemoryThrottlingSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Results.Count(x => x.SensorIdentifier == typeof(DiskThrottlingSensor).FullName.ComputeHash()));
        }
    }
}