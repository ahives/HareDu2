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
        public void Verify_probe_warning_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RedeliveredMessagesProbe(config.Diagnostics, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(RedeliveredMessagesProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var provider = _container.Resolve<IFileConfigProvider>();
            
            provider.TryGet(path, out HareDuConfig config);
            
            var probe = new RedeliveredMessagesProbe(config.Diagnostics, _container.Resolve<IKnowledgeBaseProvider>());
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 40, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(RedeliveredMessagesProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RedeliveredMessagesProbe(null, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
        }
    }
}