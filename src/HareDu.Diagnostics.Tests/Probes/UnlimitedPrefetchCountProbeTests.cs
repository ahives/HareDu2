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

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test(Description = "")]
        public void Verify_probe_yellow_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot2(0);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Identifier.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_inconclusive_condition_1()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot2(5);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Inconclusive);
            result.KB.Identifier.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }
    }
}