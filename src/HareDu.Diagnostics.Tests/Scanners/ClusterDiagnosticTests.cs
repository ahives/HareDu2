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
    using System.Collections.Generic;
    using System.Linq;
    using Diagnostics.Configuration;
    using Diagnostics.Sensors;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterDiagnosticTests
    {
        IReadOnlyList<IDiagnosticSensor> _sensors;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticScannerConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _sensors = new List<IDiagnosticSensor>
            {
                new RuntimeProcessLimitSensor(configProvider, knowledgeBaseProvider),
                new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider),
                new NetworkPartitionSensor(configProvider, knowledgeBaseProvider),
                new MemoryAlarmSensor(configProvider, knowledgeBaseProvider),
                new DiskAlarmSensor(configProvider, knowledgeBaseProvider),
                new AvailableCpuCoresSensor(configProvider, knowledgeBaseProvider),
                new FileDescriptorThrottlingSensor(configProvider, knowledgeBaseProvider),
            };
        }

        [Test]
        public void Verify_sensors_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshotSnapshot1();
            
            var report = new ClusterDiagnostic(_sensors)
                .Scan(snapshot);

            Assert.AreEqual(7, report.Count);
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(RuntimeProcessLimitSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(NetworkThrottlingSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(NetworkPartitionSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(MemoryAlarmSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(DiskAlarmSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(AvailableCpuCoresSensor).FullName.GenerateIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(FileDescriptorThrottlingSensor).FullName.GenerateIdentifier()));
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var report = new ClusterDiagnostic(_sensors)
                .Scan(snapshot);

            Assert.IsEmpty(report);
        }
    }
}