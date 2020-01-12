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
    using System.Collections.Generic;
    using System.Linq;
    using Core.Configuration;
    using Diagnostics.Probes;
    using Extensions;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterDiagnosticTests
    {
        IReadOnlyList<IDiagnosticProbe> _probes;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new ConfigurationProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _probes = new List<IDiagnosticProbe>
            {
                new RuntimeProcessLimitProbe(config.Analyzer, knowledgeBaseProvider),
                new SocketDescriptorThrottlingProbe(config.Analyzer, knowledgeBaseProvider),
                new NetworkPartitionProbe(config.Analyzer, knowledgeBaseProvider),
                new MemoryAlarmProbe(config.Analyzer, knowledgeBaseProvider),
                new DiskAlarmProbe(config.Analyzer, knowledgeBaseProvider),
                new AvailableCpuCoresProbe(config.Analyzer, knowledgeBaseProvider),
                new FileDescriptorThrottlingProbe(config.Analyzer, knowledgeBaseProvider),
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            
            var report = new ClusterDiagnostic(_probes)
                .Scan(snapshot);

            report.Count.ShouldBe(7);
            report.Count(x => x.AnalyzerIdentifier == typeof(RuntimeProcessLimitProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(SocketDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(NetworkPartitionProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(MemoryAlarmProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(DiskAlarmProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(AvailableCpuCoresProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(FileDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var report = new ClusterDiagnostic(_probes)
                .Scan(snapshot);

            report.ShouldBeEmpty();
        }
    }
}