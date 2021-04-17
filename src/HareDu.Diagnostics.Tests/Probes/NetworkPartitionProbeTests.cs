namespace HareDu.Diagnostics.Tests.Probes
{
    using System.Collections.Generic;
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
    public class NetworkPartitionProbeTests
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
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new NetworkPartitionProbe(knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot2(new List<string>
            {
                "node1@rabbitmq",
                "node2@rabbitmq",
                "node3@rabbitmq"
            });

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(NetworkPartitionProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new NetworkPartitionProbe(knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot2(new List<string>());

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(NetworkPartitionProbe).GetIdentifier());
        }
    }
}