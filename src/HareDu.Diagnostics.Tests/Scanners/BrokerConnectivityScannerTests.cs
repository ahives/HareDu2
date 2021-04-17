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
    public class BrokerConnectivityScannerTests
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
                new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new HighConnectionClosureRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnlimitedPrefetchCountProbe(knowledgeBaseProvider),
                new ChannelThrottlingProbe(knowledgeBaseProvider),
                new ChannelLimitReachedProbe(knowledgeBaseProvider),
                new BlockedConnectionProbe(knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var result = new BrokerConnectivityScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(6);
            result.Count(x => x.Id == typeof(HighConnectionCreationRateProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(HighConnectionClosureRateProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(UnlimitedPrefetchCountProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ChannelThrottlingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ChannelLimitReachedProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(BlockedConnectionProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerConnectivitySnapshot snapshot = null;
            
            var result = new BrokerConnectivityScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}