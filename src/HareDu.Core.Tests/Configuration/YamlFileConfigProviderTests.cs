namespace HareDu.Core.Tests.Configuration
{
    using Core.Configuration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class YamlFileConfigProviderTests
    {
        [Test]
        public void Verify_can_get_configuration_from_file()
        {
            var provider = new YamlFileConfigProvider();

            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            
            provider.TryGet(path, out var config).ShouldBeTrue();
            config.Broker.Url.ShouldBe("http://localhost:15672");
            config.Broker.Credentials.Username.ShouldBe("guest");
            config.Broker.Credentials.Password.ShouldBe("guest");
            config.Diagnostics.Probes.HighConnectionClosureRateThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.HighConnectionCreationRateThreshold.ShouldBe<uint>(60);
            config.Diagnostics.Probes.QueueHighFlowThreshold.ShouldBe<uint>(90);
            config.Diagnostics.Probes.QueueLowFlowThreshold.ShouldBe<uint>(10);
            config.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.60M);
            config.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.65M);
            config.Diagnostics.Probes.ConsumerUtilizationThreshold.ShouldBe(0.65M);
        }
        
        [Test]
        public void Verify_cannot_get_configuration_from_file()
        {
            var provider = new YamlFileConfigProvider();

            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_x.yaml";
            
            provider.TryGet(path, out var config).ShouldBeFalse();
            config.Broker.Url.ShouldBe("http://localhost:15672");
            config.Broker.Credentials.Username.ShouldBe("guest");
            config.Broker.Credentials.Password.ShouldBe("guest");
            config.Diagnostics.Probes.HighConnectionClosureRateThreshold.ShouldBe<uint>(100);
            config.Diagnostics.Probes.HighConnectionCreationRateThreshold.ShouldBe<uint>(100);
            config.Diagnostics.Probes.QueueHighFlowThreshold.ShouldBe<uint>(100);
            config.Diagnostics.Probes.QueueLowFlowThreshold.ShouldBe<uint>(20);
            config.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(1M);
            config.Diagnostics.Probes.SocketUsageThresholdCoefficient.ShouldBe(0.50M);
            config.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(0.7M);
            config.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(0.7M);
            config.Diagnostics.Probes.ConsumerUtilizationThreshold.ShouldBe(0.50M);
        }
    }
}