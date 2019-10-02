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
    public class BrokerQueuesDiagnosticTests
    {
        IReadOnlyList<IDiagnosticAnalyzer> _analyzers;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticScannerConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _analyzers = new List<IDiagnosticAnalyzer>
            {
                new QueueGrowthAnalyzer(configProvider, knowledgeBaseProvider),
                new MessagePagingAnalyzer(configProvider, knowledgeBaseProvider),
                new RedeliveredMessagesAnalyzer(configProvider, knowledgeBaseProvider),
                new ConsumerUtilizationAnalyzer(configProvider, knowledgeBaseProvider),
                new UnroutableMessageAnalyzer(configProvider, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_sensors_fired()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);

            var report = new BrokerQueuesDiagnostic(_analyzers)
                .Scan(snapshot);

            Assert.AreEqual(5, report.Count);
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(QueueGrowthAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(MessagePagingAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(RedeliveredMessagesAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(ConsumerUtilizationAnalyzer).GetIdentifier()));
            Assert.AreEqual(1, report.Count(x => x.AnalyzerIdentifier == typeof(UnroutableMessageAnalyzer).GetIdentifier()));
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerQueuesSnapshot snapshot = null;
            
            var report = new BrokerQueuesDiagnostic(_analyzers)
                .Scan(snapshot);

            Assert.IsEmpty(report);
        }
    }
}