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
namespace HareDu.Diagnostics.Tests
{
    using System.Collections.Generic;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Diagnostics.Analyzers;
    using KnowledgeBase;
    using NUnit.Framework;
    using Registration;
    using Scanning;
    using Shouldly;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class ComponentDiagnosticFactoryTests
    {
        IReadOnlyList<IDiagnosticAnalyzer> _analyzers;
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();

            var configProvider = _container.Resolve<IConfigurationProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();

            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            
            configProvider.TryGet(path, out HareDuConfig config);

            _analyzers = new List<IDiagnosticAnalyzer>
            {
                new HighConnectionCreationRateAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new HighConnectionClosureRateAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new UnlimitedPrefetchCountAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new ChannelThrottlingAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new ChannelLimitReachedAnalyzer(config.Analyzer, knowledgeBaseProvider),
                new BlockedConnectionAnalyzer(config.Analyzer, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_can_get_diagnostic()
        {
            // string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
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
            var factory = _container.Resolve<IComponentDiagnosticFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic).ShouldBeTrue();
            diagnostic.Identifier.ShouldBe(typeof(BrokerConnectivityDiagnostic).GetIdentifier());
        }

        [Test]
        public void Test()
        {
            var factory = _container.Resolve<IComponentDiagnosticFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic).ShouldBeTrue();
//            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            // var diagnosticsRegistrar = new ComponentDiagnosticRegistry();
            // diagnosticsRegistrar.RegisterAll();
            // var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.ObjectCache, diagnosticsRegistrar.Types, _analyzers);
            var factory = _container.Resolve<IComponentDiagnosticFactory>();

            factory.TryGet<ConnectionSnapshot>(out var diagnostic).ShouldBeFalse();
            diagnostic.Identifier.ShouldBe(typeof(NoOpDiagnostic<ConnectionSnapshot>).GetIdentifier());
        }

        [Test]
        public void Verify_can_get_diagnostic_after_instantiation()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            
            var configProvider = new ConfigurationProvider();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            var diagnosticAnalyzerRegistry = new DiagnosticAnalyzerRegistrar(config.Analyzer, knowledgeBaseProvider);
            diagnosticAnalyzerRegistry.RegisterAll();
            
            var diagnosticRegistry = new ComponentDiagnosticRegistrar(diagnosticAnalyzerRegistry);
            diagnosticRegistry.RegisterAll();
            diagnosticRegistry.Register<FakeDiagnostic>();

            var factory = new ComponentDiagnosticFactory(diagnosticAnalyzerRegistry, diagnosticRegistry);

            factory.TryGet<FakeSnapshot>(out var diagnostic).ShouldBeFalse();
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        class FakeDiagnostic :
            IComponentDiagnostic<FakeSnapshot>
        {
            public string Identifier => GetType().GetIdentifier();

            public IReadOnlyList<DiagnosticAnalyzerResult> Scan(FakeSnapshot snapshot) =>
                throw new System.NotImplementedException();
        }

        interface FakeSnapshot :
            Snapshot
        {
        }
    }
}