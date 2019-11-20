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
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerConnectivityDiagnosticTests
    {
        IReadOnlyList<IDiagnosticAnalyzer> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticScannerConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _analyzers = new List<IDiagnosticAnalyzer>
            {
                new HighConnectionCreationRateAnalyzer(configProvider, knowledgeBaseProvider),
                new HighConnectionClosureRateAnalyzer(configProvider, knowledgeBaseProvider),
                new UnlimitedPrefetchCountAnalyzer(configProvider, knowledgeBaseProvider),
                new ChannelThrottlingAnalyzer(configProvider, knowledgeBaseProvider),
                new ChannelLimitReachedAnalyzer(configProvider, knowledgeBaseProvider),
                new BlockedConnectionAnalyzer(configProvider, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var report = new BrokerConnectivityDiagnostic(_analyzers)
                .Scan(snapshot);

            report.Count.ShouldBe(6);
            report.Count(x => x.AnalyzerIdentifier == typeof(HighConnectionCreationRateAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(HighConnectionClosureRateAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(UnlimitedPrefetchCountAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(ChannelThrottlingAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(ChannelLimitReachedAnalyzer).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.AnalyzerIdentifier == typeof(BlockedConnectionAnalyzer).GetIdentifier()).ShouldBe(1);
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