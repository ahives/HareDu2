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
    public class RedeliveredMessagesProbeTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_probe_yellow_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RedeliveredMessagesProbe(config.Diagnostics, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.Warning);
            result.Article.Identifier.ShouldBe(typeof(RedeliveredMessagesProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_green_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var provider = _container.Resolve<IFileConfigProvider>();
            
            provider.TryGet(path, out HareDuConfig config);
            
            var probe = new RedeliveredMessagesProbe(config.Diagnostics, _container.Resolve<IKnowledgeBaseProvider>());
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 40, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.Healthy);
            result.Article.Identifier.ShouldBe(typeof(RedeliveredMessagesProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_offline()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RedeliveredMessagesProbe(null, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.NA);
        }
    }
}