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
namespace HareDu.Diagnostics.Tests.Analyzers
{
    using Autofac;
    using Diagnostics.Analyzers;
    using Diagnostics.Configuration;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class MemoryAlarmAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultKnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScannerConfigProvider>()
                .As<IDiagnosticScannerConfigProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_analyzer_red_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new MemoryAlarmAnalyzer(configProvider, knowledgeBaseProvider);
            
            MemorySnapshot snapshot = new FakeMemorySnapshot1(103283, 823983, true);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Red);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(MemoryAlarmAnalyzer).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_green_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new MemoryAlarmAnalyzer(configProvider, knowledgeBaseProvider);
            
            MemorySnapshot snapshot = new FakeMemorySnapshot1(103283, 823983, false);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Green);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(MemoryAlarmAnalyzer).GetIdentifier());
        }
    }
}