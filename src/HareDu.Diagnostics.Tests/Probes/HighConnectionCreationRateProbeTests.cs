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
    public class HighConnectionCreationRateProbeTests
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
        public void Verify_probe_warning_condition_1()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(102, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_warning_condition_2()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(100, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var configProvider = _container.Resolve<IFileConfigProvider>();
            configProvider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(99, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var probe = new HighConnectionCreationRateProbe(null, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(99, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
        }
    }
}