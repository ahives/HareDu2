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
    using Core.Extensions;
    using Diagnostics.Probes;
    using Diagnostics.Scanners;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ClusterScannerTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;

        [OneTimeSetUp]
        public void Init()
        {
            var validator = new HareDuConfigValidator();
            var configProvider = new YamlFileConfigProvider(validator);
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _probes = new List<DiagnosticProbe>
            {
                new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider),
                new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider),
                new NetworkPartitionProbe(knowledgeBaseProvider),
                new MemoryAlarmProbe(knowledgeBaseProvider),
                new DiskAlarmProbe(knowledgeBaseProvider),
                new AvailableCpuCoresProbe(knowledgeBaseProvider),
                new FileDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider),
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            
            var result = new ClusterScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(7);
            result.Count(x => x.Id == typeof(RuntimeProcessLimitProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(SocketDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(NetworkPartitionProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(MemoryAlarmProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(DiskAlarmProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(AvailableCpuCoresProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(FileDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var result = new ClusterScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}