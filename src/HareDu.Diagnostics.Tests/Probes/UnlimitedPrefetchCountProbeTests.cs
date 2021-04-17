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
        public void Verify_probe_warning_condition()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot2(0);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_inconclusive_condition_1()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot2(5);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Inconclusive);
            result.KB.Id.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }
    }
}