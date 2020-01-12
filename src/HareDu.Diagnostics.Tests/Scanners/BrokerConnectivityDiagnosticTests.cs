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
    public class BrokerConnectivityDiagnosticTests
    {
        IReadOnlyList<IDiagnosticProbe> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new ConfigurationProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _analyzers = new List<IDiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Analyzer, knowledgeBaseProvider),
                new HighConnectionClosureRateProbe(config.Analyzer, knowledgeBaseProvider),
                new UnlimitedPrefetchCountProbe(config.Analyzer, knowledgeBaseProvider),
                new ChannelThrottlingProbe(config.Analyzer, knowledgeBaseProvider),
                new ChannelLimitReachedProbe(config.Analyzer, knowledgeBaseProvider),
                new BlockedConnectionProbe(config.Analyzer, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var report = new BrokerConnectivityDiagnostic(_analyzers)
                .Scan(snapshot);

            report.Count.ShouldBe(6);
            report.Count(x => x.AnalyzerIdentifier == typeof(HighConnectionCreationRateProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(HighConnectionClosureRateProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(UnlimitedPrefetchCountProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(ChannelThrottlingProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(ChannelLimitReachedProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(BlockedConnectionProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerConnectivitySnapshot snapshot = null;
            
            var report = new BrokerConnectivityDiagnostic(_analyzers)
                .Scan(snapshot);

            report.ShouldBeEmpty();
        }
    }
}