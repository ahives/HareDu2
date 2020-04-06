// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Diagnostics.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Diagnostics.Registration;
    using Diagnostics.Scanners;
    using KnowledgeBase;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class ScannerFactoryTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .AddHareDuDiagnostics()
                .Build();

            var provider = _container.Resolve<IFileConfigProvider>();
            var kb = _container.Resolve<IKnowledgeBaseProvider>();

            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            
            provider.TryGet(path, out HareDuConfig config);

            _probes = new List<DiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Diagnostics, kb),
                new HighConnectionClosureRateProbe(config.Diagnostics, kb),
                new UnlimitedPrefetchCountProbe(kb),
                new ChannelThrottlingProbe(kb),
                new ChannelLimitReachedProbe(kb),
                new BlockedConnectionProbe(kb)
            };
        }

        [Test]
        public void Verify_can_get_diagnostic()
        {
            // string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            //
            // var configProvider = new ConfigurationProvider();
            // configProvider.TryGet(path, out HareDuConfig config);
            //
            // var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            // var diagnosticAnalyzerRegistry = new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            // diagnosticAnalyzerRegistry.RegisterAll();
            //
            // var diagnosticsRegistrar = new ComponentDiagnosticRegistry(diagnosticAnalyzerRegistry.ObjectCache);
            // diagnosticsRegistrar.RegisterAll();
            //
            // var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.ObjectCache, diagnosticsRegistrar.Types, _analyzers);
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic).ShouldBeTrue();
            diagnostic.Identifier.ShouldBe(typeof(BrokerConnectivityScanner).GetIdentifier());
        }

        [Test]
        public void Test()
        {
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out _).ShouldBeTrue();
//            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            var factory = _container.Resolve<IScannerFactory>();

            factory.TryGet<ConnectionSnapshot>(out var diagnostic).ShouldBeFalse();
            diagnostic.Identifier.ShouldBe(typeof(NoOpScanner<ConnectionSnapshot>).GetIdentifier());
        }

        [Test]
        public void Verify_can_get_diagnostic_after_instantiation()
        {
            string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            var provider = new YamlFileConfigProvider();
            provider.TryGet(path, out HareDuConfig config);
            
            var knowledgeBaseProvider = new KnowledgeBaseProvider();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);

            factory.TryGet<FakeSnapshot>(out _).ShouldBeFalse();
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_can_add_new_probes()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);

            bool registered = factory.RegisterProbe(new FakeProbe(5, _container.Resolve<IKnowledgeBaseProvider>()));
            registered.ShouldBeTrue();
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(22);
        }

        [Test]
        public void Verify_can_return_all_probes_1()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_probes_2()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllProbes();
            
            registered.ShouldBeTrue();
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_scanners_1()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_return_all_scanners_2()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());

            bool registered = factory.TryRegisterAllScanners();
            
            registered.ShouldBeTrue();
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_register_new_scanner()
        {
            var provider = new YamlFileConfigProvider();
            provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);

            var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);

            bool registered = factory.RegisterScanner(new FakeDiagnosticScanner());
            registered.ShouldBeTrue();
            registered.ShouldBeTrue();
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(4);
        }

        [Test]
        public void Verify_can_override_config()
        {
            var configProvider = new YamlFileConfigProvider();
            configProvider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config1);

            var factory = new ScannerFactory(config1, new KnowledgeBaseProvider());

            factory.RegisterObserver(new ConfigOverrideObserver());

            var provider = new HareDuConfigProvider();

            var config2 = provider.Configure(x =>
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
            
            factory.UpdateConfiguration(config2);
        }

        
        class ConfigOverrideObserver :
            IObserver<ProbeConfigurationContext>
        {
            public void OnCompleted() => throw new NotImplementedException();

            public void OnError(Exception error) => throw new NotImplementedException();

            public void OnNext(ProbeConfigurationContext value)
            {
                value.Current.Probes.HighConnectionCreationRateThreshold.ShouldNotBe(value.New.Probes.HighConnectionCreationRateThreshold);
                value.Current.Probes.HighConnectionClosureRateThreshold.ShouldNotBe(value.New.Probes.HighConnectionClosureRateThreshold);
                value.Current.Probes.ConsumerUtilizationThreshold.ShouldNotBe(value.New.Probes.ConsumerUtilizationThreshold);
                value.Current.Probes.MessageRedeliveryThresholdCoefficient.ShouldNotBe(value.New.Probes.MessageRedeliveryThresholdCoefficient);
                value.Current.Probes.QueueHighFlowThreshold.ShouldNotBe(value.New.Probes.QueueHighFlowThreshold);
                value.Current.Probes.QueueLowFlowThreshold.ShouldNotBe(value.New.Probes.QueueLowFlowThreshold);
                value.Current.Probes.SocketUsageThresholdCoefficient.ShouldBe(value.New.Probes.SocketUsageThresholdCoefficient);
                value.Current.Probes.FileDescriptorUsageThresholdCoefficient.ShouldBe(value.New.Probes.FileDescriptorUsageThresholdCoefficient);
                value.Current.Probes.RuntimeProcessUsageThresholdCoefficient.ShouldBe(value.New.Probes.RuntimeProcessUsageThresholdCoefficient);
            }
        }

        
        class FakeDiagnosticScanner :
            DiagnosticScanner<FakeSnapshot>
        {
            public string Identifier => GetType().GetIdentifier();

            public IReadOnlyList<ProbeResult> Scan(FakeSnapshot snapshot) =>
                throw new System.NotImplementedException();
        }

        interface FakeSnapshot :
            Snapshot
        {
        }

        
        class FakeProbe :
            BaseDiagnosticProbe,
            DiagnosticProbe
        {
            public FakeProbe(int someConfigValue, IKnowledgeBaseProvider kb)
                : base(kb)
            {
            }

            public string Id { get; }
            public string Name { get; }
            public string Description { get; }
            public ComponentType ComponentType { get; }
            public ProbeCategory Category { get; }
            public ProbeStatus Status { get; }
            public ProbeResult Execute<T>(T snapshot) => throw new NotImplementedException();
        }
    }
}