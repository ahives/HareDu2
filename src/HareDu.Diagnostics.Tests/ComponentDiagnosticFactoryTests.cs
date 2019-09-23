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
    using Analyzers;
    using Autofac;
    using AutofacIntegration;
    using Diagnostics.Analyzers;
    using Diagnostics.Configuration;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
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
        public void Verify_can_get_diagnostic()
        {
            var diagnosticsRegistrar = new ComponentDiagnosticRegistrar();
            diagnosticsRegistrar.RegisterAll(_analyzers);
            var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.Cache, diagnosticsRegistrar.Types, _analyzers);
            
            Assert.IsTrue(factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic));
            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Test()
        {
            var factory = _container.Resolve<IComponentDiagnosticFactory>();
            
            Assert.IsTrue(factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic));
//            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            var diagnosticsRegistrar = new ComponentDiagnosticRegistrar();
            diagnosticsRegistrar.RegisterAll(_analyzers);
            var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.Cache, diagnosticsRegistrar.Types, _analyzers);
            
            Assert.IsFalse(factory.TryGet<ConnectionSnapshot>(out var diagnostic));
            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_can_get_diagnostic_after_instantiation()
        {
            var diagnosticsRegistrar = new ComponentDiagnosticRegistrar();
            diagnosticsRegistrar.RegisterAll(_analyzers);
            diagnosticsRegistrar.Register<FakeDiagnostic>(_analyzers);
            
            var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.Cache, diagnosticsRegistrar.Types, _analyzers);
            
            Assert.IsFalse(factory.TryGet<FakeSnapshot>(out var diagnostic));
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }
    }

    class FakeDiagnostic :
        IComponentDiagnostic<FakeSnapshot>
    {
        public string Identifier => GetType().GenerateIdentifier();
        
        public IReadOnlyList<DiagnosticResult> Scan(FakeSnapshot snapshot) => throw new System.NotImplementedException();
    }

    interface FakeSnapshot :
        Snapshot
    {
    }
}