namespace HareDu.Core.Tests.Configuration
{
    using Core.Configuration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class HareDuConfigProviderTests
    {
        [Test]
        public void Verify_can_configure_diagnostic_probes()
        {
            var provider = new HareDuConfigProvider();

            var config = provider.Configure(x =>
            {
                x.Broker(y =>
                {
                    y.ConnectTo("http://localhost:15672");
                    y.UsingCredentials("guest", "guest");
                });

                x.Diagnostics(y =>
                {
                    y.Probes(z =>
                    {
                        z.SetMessageRedeliveryThresholdCoefficient(0.60M);
                        z.SetSocketUsageThresholdCoefficient(0.60M);
                        z.SetConsumerUtilizationThreshold(0.65M);
                        z.SetQueueHighFlowThreshold(90);
                        z.SetQueueLowFlowThreshold(10);
                        z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                        z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                        z.SetHighConnectionClosureRateThreshold(90);
                        z.SetHighConnectionCreationRateThreshold(60);
                    });
                });
            });
            
            config.Broker.Url.ShouldBe("http://localhost:15670");
            config.Broker.Credentials.ShouldNotBeNull();
            config.Broker.Credentials.Username.ShouldBe("guest1");
            config.Broker.Credentials.Password.ShouldBe("guest1");
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
        public void Verify_cannot_configure_diagnostic_probes()
        {
            var provider = new HareDuConfigProvider();

            var config = provider.Configure(x =>
                                         {
                                         });
            
            config.Broker.Url.ShouldBe(ConfigCache.Default.Broker.Url);
            config.Broker.Credentials.Username.ShouldBe(ConfigCache.Default.Broker.Credentials.Username);
            config.Broker.Credentials.Password.ShouldBe(ConfigCache.Default.Broker.Credentials.Password);
            config.Diagnostics.Probes.HighConnectionClosureRateThreshold.ShouldBe<uint>(ConfigCache.Default.Diagnostics.Probes.HighConnectionClosureRateThreshold);
            config.Diagnostics.Probes.HighConnectionCreationRateThreshold.ShouldBe<uint>(ConfigCache.Default.Diagnostics.Probes.HighConnectionCreationRateThreshold);
            config.Diagnostics.Probes.QueueHighFlowThreshold.ShouldBe<uint>(ConfigCache.Default.Diagnostics.Probes.QueueHighFlowThreshold);
            config.Diagnostics.Probes.QueueLowFlowThreshold.ShouldBe<uint>(ConfigCache.Default.Diagnostics.Probes.QueueLowFlowThreshold);
            config.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient.ShouldBe(ConfigCache.Default.Diagnostics.Probes.MessageRedeliveryThresholdCoefficient);
            config.Diagnostics.Probes.SocketUsageThresholdCoefficient.ShouldBe(ConfigCache.Default.Diagnostics.Probes.SocketUsageThresholdCoefficient);
            config.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(ConfigCache.Default.Diagnostics.Probes.RuntimeProcessUsageThresholdCoefficient);
            config.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(ConfigCache.Default.Diagnostics.Probes.FileDescriptorUsageThresholdCoefficient);
            config.Diagnostics.Probes.ConsumerUtilizationThreshold.ShouldBe(ConfigCache.Default.Diagnostics.Probes.ConsumerUtilizationThreshold);
        }
    }
}