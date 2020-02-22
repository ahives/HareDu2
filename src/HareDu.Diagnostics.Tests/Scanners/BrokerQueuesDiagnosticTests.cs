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
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerQueuesDiagnosticTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new YamlConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);
            
            _probes = new List<DiagnosticProbe>
            {
                new QueueGrowthProbe(knowledgeBaseProvider),
                new MessagePagingProbe(knowledgeBaseProvider),
                new RedeliveredMessagesProbe(config.Diagnostics, knowledgeBaseProvider),
                new ConsumerUtilizationProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnroutableMessageProbe(knowledgeBaseProvider),
                new QueueLowFlowProbe(config.Diagnostics, knowledgeBaseProvider),
                new QueueNoFlowProbe(knowledgeBaseProvider),
                new QueueHighFlowProbe(config.Diagnostics, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);

            var report = new BrokerQueuesDiagnostic(_probes)
                .Scan(snapshot);

            report.Count.ShouldBe(8);
            report.Count(x => x.ProbeIdentifier == typeof(QueueGrowthProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(MessagePagingProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(RedeliveredMessagesProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(ConsumerUtilizationProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(UnroutableMessageProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(QueueLowFlowProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(QueueNoFlowProbe).GetIdentifier()).ShouldBe(1);
            report.Count(x => x.ProbeIdentifier == typeof(QueueHighFlowProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerQueuesSnapshot snapshot = null;
            
            var report = new BrokerQueuesDiagnostic(_probes)
                .Scan(snapshot);

            report.ShouldBeEmpty();
        }
    }
}