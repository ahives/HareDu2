namespace HareDu.Diagnostics.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Diagnostics.Scanners;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class ScannerFactoryTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .Build();

            HareDuConfig config = _container.Resolve<HareDuConfig>();
            var kb = _container.Resolve<IKnowledgeBaseProvider>();
            
            _probes = new List<DiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Diagnostics, kb),
                new HighConnectionClosureRateProbe(config.Diagnostics, kb),
                new UnlimitedPrefetchCountProbe(kb),
                new ChannelThrottlingProbe(kb),
                new ChannelLimitReachedProbe(kb),
                new BlockedConnectionProbe(kb)
            };
        }

        [Test]
        public void Verify_can_get_diagnostic()
        {
            // string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            //
            // var configProvider = new ConfigurationProvider();
            // configProvider.TryGet(path, out HareDuConfig config);
            //
            // var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            // var diagnosticAnalyzerRegistry = new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            // diagnosticAnalyzerRegistry.RegisterAll();
            //
            // var diagnosticsRegistrar = new ComponentDiagnosticRegistry(diagnosticAnalyzerRegistry.ObjectCache);
            // diagnosticsRegistrar.RegisterAll();
            //
            // var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.ObjectCache, diagnosticsRegistrar.Types, _analyzers);
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic).ShouldBeTrue();
            diagnostic.Identifier.ShouldBe(typeof(BrokerConnectivityScanner).GetIdentifier());
        }

        [Test]
        public void Test()
        {
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out _).ShouldBeTrue();
//            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<ConnectionSnapshot>(out var diagnostic).ShouldBeFalse();
            diagnostic.Identifier.ShouldBe(typeof(NoOpScanner<ConnectionSnapshot>).GetIdentifier());
        }

        [Test]
        public void Verify_can_get_diagnostic_after_instantiation()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var provider = new YamlFileConfigProvider();
            provider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);

            factory.TryGet<FakeSnapshot>(out _).ShouldBeFalse();
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_can_add_new_probes()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);

            bool registered = factory.RegisterProbe(new FakeProbe(5, _container.Resolve<IKnowledgeBaseProvider>()));
            registered.ShouldBeTrue();
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(22);
        }

        [Test]
        public void Verify_can_return_all_probes_1()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_probes_2()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllProbes();
            
            registered.ShouldBeTrue();
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_scanners_1()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_return_all_scanners_2()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllScanners();
            
            registered.ShouldBeTrue();
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_register_new_scanner()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);

            bool registered = factory.RegisterScanner(new FakeDiagnosticScanner());
            registered.ShouldBeTrue();
            registered.ShouldBeTrue();
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(4);
        }

        
        class FakeDiagnosticScanner :
            DiagnosticScanner<FakeSnapshot>
        {
            public string Identifier => GetType().GetIdentifier();

            public IReadOnlyList<ProbeResult> Scan(FakeSnapshot snapshot) =>
                throw new System.NotImplementedException();
        }

        interface FakeSnapshot :
            Snapshot
        {
        }

        
        class FakeProbe :
            BaseDiagnosticProbe,
            DiagnosticProbe
        {
            public FakeProbe(int someConfigValue, IKnowledgeBaseProvider kb)
                : base(kb)
            {
            }

            public DiagnosticProbeMetadata Metadata { get; }
            public ComponentType ComponentType { get; }
            public ProbeCategory Category { get; }
            public ProbeStatus Status { get; }
            public ProbeResult Execute<T>(T snapshot) => throw new NotImplementedException();
        }
    }
}