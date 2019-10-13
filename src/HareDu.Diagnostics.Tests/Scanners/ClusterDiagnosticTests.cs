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
    using Analyzers;
    using Diagnostics.Analyzers;
    using Diagnostics.Configuration;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterDiagnosticTests
    {
        IReadOnlyList<IDiagnosticAnalyzer> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticScannerConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _analyzers = new List<IDiagnosticAnalyzer>
            {
                new RuntimeProcessLimitAnalyzer(configProvider, knowledgeBaseProvider),
                new SocketDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider),
                new NetworkPartitionAnalyzer(configProvider, knowledgeBaseProvider),
                new MemoryAlarmAnalyzer(configProvider, knowledgeBaseProvider),
                new DiskAlarmAnalyzer(configProvider, knowledgeBaseProvider),
                new AvailableCpuCoresAnalyzer(configProvider, knowledgeBaseProvider),
                new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider),
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            
            var report = new ClusterDiagnostic(_analyzers)
                .Scan(snapshot);

            Assert.AreEqual(7, report.Count);
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(RuntimeProcessLimitAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(SocketDescriptorThrottlingAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(NetworkPartitionAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(MemoryAlarmAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(DiskAlarmAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(AvailableCpuCoresAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier()));
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var report = new ClusterDiagnostic(_analyzers)
                .Scan(snapshot);

            Assert.IsEmpty(report);
        }
    }
}