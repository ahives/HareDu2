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
    public class SocketDescriptorThrottlingProbeTests
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
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 9, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Identifier.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test(Description = "When sockets used >= calculated high watermark and calculated high watermark >= max sockets available")]
        public void Verify_probe_red_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 10, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Identifier.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_green_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Identifier.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_offline()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(null, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
            result.KB.Identifier.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }
    }
}