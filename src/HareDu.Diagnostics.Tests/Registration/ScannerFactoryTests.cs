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
namespace HareDu.Diagnostics.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Diagnostics.Registration;
    using Diagnostics.Scanners;
    using KnowledgeBase;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting;
    using Snapshotting.Model;
    using Snapshotting.Persistence;

    [TestFixture]
    public class ScannerFactoryTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();

            var configProvider = _container.Resolve<IFileConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();

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
            
            var validator = new HareDuConfigValidator();
            var configProvider = new YamlFileConfigProvider(validator);
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            var factory = new ScannerFactory(config.Diagnostics, knowledgeBaseProvider);

            factory.TryGet<FakeSnapshot>(out _).ShouldBeFalse();
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_can_add_new_probes()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());
            
            var probes1 = factory.GetProbes();
            
            probes1.ShouldNotBeNull();
            probes1.ShouldNotBeEmpty();
            probes1.Keys.Count().ShouldBe(21);

            bool registered = factory.RegisterProbe(new FakeProbe(5, _container.Resolve<IKnowledgeBaseProvider>()));
            registered.ShouldBeTrue();
            
            var probes2 = factory.GetProbes();
            
            probes2.ShouldNotBeNull();
            probes2.ShouldNotBeEmpty();
            probes2.Keys.Count().ShouldBe(22);
        }

        [Test]
        public void Verify_can_return_all_probes_1()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());

            var probes = factory.GetProbes();
            
            probes.ShouldNotBeNull();
            probes.ShouldNotBeEmpty();
            probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_probes_2()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllProbes();
            var probes = factory.GetProbes();
            
            registered.ShouldBeTrue();
            probes.ShouldNotBeNull();
            probes.ShouldNotBeEmpty();
            probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_scanners_1()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());

            var scanners = factory.GetScanners();
            
            scanners.ShouldNotBeNull();
            scanners.ShouldNotBeEmpty();
            scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_return_all_scanners_2()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllScanners();
            
            var scanners = factory.GetScanners();
            
            registered.ShouldBeTrue();
            scanners.ShouldNotBeNull();
            scanners.ShouldNotBeEmpty();
            scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_register_new_scanner()
        {
            var configProvider = new YamlFileConfigProvider(new HareDuConfigValidator());
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config.Diagnostics, new KnowledgeBaseProvider());
            
            var scanners1 = factory.GetScanners();
            
            scanners1.ShouldNotBeNull();
            scanners1.ShouldNotBeEmpty();
            scanners1.Keys.Count().ShouldBe(3);

            bool registered = factory.RegisterScanner(new FakeDiagnosticScanner());
            registered.ShouldBeTrue();
            registered.ShouldBeTrue();
            
            var scanners2 = factory.GetScanners();
            
            scanners2.ShouldNotBeNull();
            scanners2.ShouldNotBeEmpty();
            scanners2.Keys.Count().ShouldBe(4);
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

            public IDisposable Subscribe(IObserver<ProbeContext> observer) => throw new NotImplementedException();

            public string Id { get; }
            public string Name { get; }
            public string Description { get; }
            public ComponentType ComponentType { get; }
            public ProbeCategory Category { get; }
            public ProbeStatus Status { get; }
            public ProbeResult Execute<T>(T snapshot) => throw new NotImplementedException();
        }
    }
}