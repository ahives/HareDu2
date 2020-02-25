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
    public class MessagePagingProbeTests
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
        public void Verify_analyzer_red_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new MessagePagingProbe(knowledgeBaseProvider);

            QueueSnapshot snapshot = new FakeQueueSnapshot4(3);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Unhealthy);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(MessagePagingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_green_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new MessagePagingProbe(knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot4(0);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(DiagnosticStatus.Healthy);
            result.KnowledgeBaseArticle.Identifier.ShouldBe(typeof(MessagePagingProbe).GetIdentifier());
        }
    }
}