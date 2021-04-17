namespace HareDu.Diagnostics.Tests.Probes
{
    using Autofac;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ChannelThrottlingProbeTests
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
            var probe = new ChannelThrottlingProbe(knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot1("Channel1", 4, 2, 5, 8, 6, 1);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(ChannelThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new ChannelThrottlingProbe(knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot1("Channel1", 6, 2, 5, 8, 4, 1);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(ChannelThrottlingProbe).GetIdentifier());
        }
    }
}