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
    using Core.Configuration;
    using Diagnostics.Analyzers;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ChannelLimitReachedAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultKnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_analyzer_red_condition_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = _container.Resolve<IConfigurationProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new ChannelLimitReachedAnalyzer(config.Analyzer, knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(3, 2);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Red);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(ChannelLimitReachedAnalyzer).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_red_condition_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = _container.Resolve<IConfigurationProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new ChannelLimitReachedAnalyzer(config.Analyzer, knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(3, 3);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Red);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(ChannelLimitReachedAnalyzer).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_green_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = _container.Resolve<IConfigurationProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new ChannelLimitReachedAnalyzer(config.Analyzer, knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(2, 3);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Green);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(ChannelLimitReachedAnalyzer).GetIdentifier());
        }
    }
}