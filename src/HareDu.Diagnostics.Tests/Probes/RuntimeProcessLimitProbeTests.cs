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
    public class RuntimeProcessLimitProbeTests
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
        public void Verify_probe_unhealthy_condition_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test(Description = "")]
        public void Verify_probe_unhealthy_condition_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 4, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(100, 40, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_warning_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(null, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }
    }
}