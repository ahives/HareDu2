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
    public class RedeliveredMessagesAnalyzerTests
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
        public void Verify_analyzer_yellow_condition()
        {
            var configProvider = new DefaultConfigProvider1();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new RedeliveredMessagesAnalyzer(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Yellow);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(RedeliveredMessagesAnalyzer).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_green_condition()
        {
            var configProvider = new DefaultConfigProvider1();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new RedeliveredMessagesAnalyzer(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 50, 32.3M);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Green);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(RedeliveredMessagesAnalyzer).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_offline()
        {
            var configProvider = new DefaultConfigProvider();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new RedeliveredMessagesAnalyzer(configProvider, knowledgeBaseProvider);
            
            analyzer.Status.ShouldBe(DiagnosticAnalyzerStatus.Offline);
        }

        
        class DefaultConfigProvider1 :
            IDiagnosticScannerConfigProvider
        {
            public bool TryGet(out DiagnosticScannerConfig config)
            {
                config = new FakeDiagnosticScannerConfig();
                return true;
            }


            class FakeDiagnosticScannerConfig :
                DiagnosticScannerConfig
            {
                public FakeDiagnosticScannerConfig()
                {
                    Analyzer = new FakeDiagnosticAnalyzerConfig();
                }

                public bool OverrideAnalyzerConfig { get; }
                public DiagnosticAnalyzerConfig Analyzer { get; }


                class FakeDiagnosticAnalyzerConfig :
                    DiagnosticAnalyzerConfig
                {
                    public FakeDiagnosticAnalyzerConfig()
                    {
                        SocketUsageCoefficient = 1.0M;
                        MessageRedeliveryCoefficient = 0.8M;
                    }

                    public uint HighClosureRateWarningThreshold { get; }
                    public uint HighCreationRateWarningThreshold { get; }
                    public uint QueueHighFlowThreshold { get; }
                    public uint QueueLowFlowThreshold { get; }
                    public decimal MessageRedeliveryCoefficient { get; }
                    public decimal SocketUsageCoefficient { get; }
                    public decimal RuntimeProcessUsageCoefficient { get; }
                    public decimal FileDescriptorUsageWarningCoefficient { get; }
                    public decimal ConsumerUtilizationWarningCoefficient { get; }
                }
            }
        }

        
        class DefaultConfigProvider :
            IDiagnosticScannerConfigProvider
        {
            public bool TryGet(out DiagnosticScannerConfig config)
            {
                config = null;
                return false;
            }
        }
    }
}