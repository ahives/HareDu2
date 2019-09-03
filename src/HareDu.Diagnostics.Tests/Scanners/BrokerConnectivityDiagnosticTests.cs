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
    using Configuration;
    using Diagnostics.Sensors;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerConnectivityDiagnosticTests
    {
        IReadOnlyList<IDiagnosticSensor> _sensors;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticSensorConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _sensors = new List<IDiagnosticSensor>
            {
                new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider),
                new HighConnectionClosureRateSensor(configProvider, knowledgeBaseProvider),
                new UnlimitedPrefetchCountSensor(configProvider, knowledgeBaseProvider),
                new ChannelThrottlingSensor(configProvider, knowledgeBaseProvider),
                new ChannelLimitReachedSensor(configProvider, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_sensors_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var report = new BrokerConnectivityDiagnostic(_sensors)
                .Scan(snapshot);

            Assert.AreEqual(5, report.Count);
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(HighConnectionCreationRateSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(HighConnectionClosureRateSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(UnlimitedPrefetchCountSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(ChannelThrottlingSensor).FullName.ComputeHash()));
            Assert.AreEqual(1, report.Count(x => x.SensorIdentifier == typeof(ChannelLimitReachedSensor).FullName.ComputeHash()));
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerConnectivitySnapshot snapshot = null;
            
            var report = new BrokerConnectivityDiagnostic(_sensors)
                .Scan(snapshot);

            Assert.IsEmpty(report);
        }
    }
}