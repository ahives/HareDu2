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
    public class BrokerQueuesScannerTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new YamlFileConfigProvider();
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            
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

            var result = new BrokerQueuesScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(8);
            result.Count(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(MessagePagingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerQueuesSnapshot snapshot = null;
            
            var result = new BrokerQueuesScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}