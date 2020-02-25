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
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scans;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerConnectivityDiagnosticTests
    {
        IReadOnlyList<DiagnosticProbe> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var validator = new HareDuConfigValidator();
            var configProvider = new YamlFileConfigProvider(validator);
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _analyzers = new List<DiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new HighConnectionClosureRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnlimitedPrefetchCountProbe(config.Diagnostics, knowledgeBaseProvider),
                new ChannelThrottlingProbe(knowledgeBaseProvider),
                new ChannelLimitReachedProbe(knowledgeBaseProvider),
                new BlockedConnectionProbe(knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var report = new BrokerConnectivityScan(_analyzers)
                .Scan(snapshot);

            report.Count.ShouldBe(6);
            report.Count(x => x.ProbeIdentifier == typeof(HighConnectionCreationRateProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(HighConnectionClosureRateProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(UnlimitedPrefetchCountProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(ChannelThrottlingProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(ChannelLimitReachedProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(BlockedConnectionProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerConnectivitySnapshot snapshot = null;
            
            var report = new BrokerConnectivityScan(_analyzers)
                .Scan(snapshot);

            report.ShouldBeEmpty();
        }
    }
}