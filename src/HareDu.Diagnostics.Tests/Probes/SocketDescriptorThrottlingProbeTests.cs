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
        public void Verify_probe_warning_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 9, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test(Description = "When sockets used >= calculated high watermark and calculated high watermark >= max sockets available")]
        public void Verify_probe_unhealthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 10, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new SocketDescriptorThrottlingProbe(null, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
            result.KB.Id.ShouldBe(typeof(SocketDescriptorThrottlingProbe).GetIdentifier());
        }
    }
}