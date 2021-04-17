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
    public class ChannelLimitReachedProbeTests
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
        public void Verify_probe_unhealthy_condition_1()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new ChannelLimitReachedProbe(knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(3, 2);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(ChannelLimitReachedProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_unhealthy_condition_2()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new ChannelLimitReachedProbe(knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(3, 3);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(ChannelLimitReachedProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new ChannelLimitReachedProbe(knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot1(2, 3);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(ChannelLimitReachedProbe).GetIdentifier());
        }
    }
}