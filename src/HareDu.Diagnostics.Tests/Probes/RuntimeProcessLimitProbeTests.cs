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
    public class RuntimeProcessLimitProbeTests
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

        [Test(Description = "")]
        public void Verify_probe_red_condition_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.Unhealthy);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test(Description = "")]
        public void Verify_probe_red_condition_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 4, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.Unhealthy);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_green_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticProbeResultStatus.Healthy);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_offline()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(null, knowledgeBaseProvider);
            
            probe.Status.ShouldBe(ProbeStatus.Offline);
        }
    }
}