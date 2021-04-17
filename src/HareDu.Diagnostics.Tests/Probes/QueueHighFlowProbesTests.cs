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
    public class QueueHighFlowProbesTests
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
        public void Verify_probe_unhealthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new QueueHighFlowProbe(config.Diagnostics, knowledgeBaseProvider);

            QueueSnapshot snapshot = new FakeQueueSnapshot5(105);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(QueueHighFlowProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new QueueHighFlowProbe(config.Diagnostics, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot5(90);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(QueueHighFlowProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new QueueHighFlowProbe(null, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot5(90);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
        }
    }
}