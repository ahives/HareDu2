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
    public class QueueGrowthProbeTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_probe_yellow_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new QueueGrowthProbe(knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot1(103283, 8734.5M, 823983, 8423.5M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Identifier.ShouldBe(typeof(QueueGrowthProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_green_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new QueueGrowthProbe(knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot1(103283, 8423.5M, 823983, 8734.5M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Identifier.ShouldBe(typeof(QueueGrowthProbe).GetIdentifier());
        }
    }
}