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
    using Core.Configuration;
    using Diagnostics.Analyzers;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterDiagnosticTests
    {
        IReadOnlyList<IDiagnosticAnalyzer> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new ConfigurationProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _analyzers = new List<IDiagnosticAnalyzer>
            {
                new RuntimeProcessLimitAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new SocketDescriptorThrottlingAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new NetworkPartitionAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new MemoryAlarmAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new DiskAlarmAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new AvailableCpuCoresAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new FileDescriptorThrottlingAnalyzer(config.Analyzer, knowledgeBaseProvider),
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            
            var report = new ClusterDiagnostic(_analyzers)
                .Scan(snapshot);

            report.Count.ShouldBe(7);
            report.Count(x => x.AnalyzerIdentifier == typeof(RuntimeProcessLimitAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(SocketDescriptorThrottlingAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(NetworkPartitionAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(MemoryAlarmAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(DiskAlarmAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(AvailableCpuCoresAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var report = new ClusterDiagnostic(_analyzers)
                .Scan(snapshot);

            report.ShouldBeEmpty();
        }
    }
}