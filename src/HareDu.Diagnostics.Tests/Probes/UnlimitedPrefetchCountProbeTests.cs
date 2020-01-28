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
namespace HareDu.Diagnostics.Tests.Probes
{
    using Autofac;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class UnlimitedPrefetchCountProbeTests
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

        [Test(Description = "")]
        public void Verify_analyzer_yellow_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = _container.Resolve<IConfigurationProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new UnlimitedPrefetchCountProbe(config.Diagnostics, knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot2(0);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Yellow);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_inconclusive_condition_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            var configProvider = _container.Resolve<IConfigurationProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var analyzer = new UnlimitedPrefetchCountProbe(config.Diagnostics, knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot2(5);

            var result = analyzer.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Inconclusive);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }
    }
}