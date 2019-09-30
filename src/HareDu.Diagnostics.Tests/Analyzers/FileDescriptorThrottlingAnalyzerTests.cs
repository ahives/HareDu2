// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Diagnostics.Tests.Analyzers
{
    using Autofac;
    using Diagnostics.Analyzers;
    using Diagnostics.Configuration;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Snapshotting.Model;

    public class FileDescriptorThrottlingAnalyzerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultKnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScannerConfigProvider>()
                .As<IDiagnosticScannerConfigProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_sensor_yellow_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 90);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Yellow,result.Status);
            Assert.AreEqual(typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test]
        public void Verify_sensor_red_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 100);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Red,result.Status);
            Assert.AreEqual(typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test]
        public void Verify_sensor_green_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 60);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
            Assert.AreEqual(typeof(FileDescriptorThrottlingAnalyzer).GetIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        [Test]
        public void Verify_sensor_offline()
        {
            var configProvider = new DefaultConfigProvider();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new FileDescriptorThrottlingAnalyzer(configProvider, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticAnalyzerStatus.Offline,sensor.Status);
        }

        
        class DefaultConfigProvider :
            IDiagnosticScannerConfigProvider
        {
            public bool TryGet(out DiagnosticScannerConfig config)
            {
                config = null;
                return false;
            }
        }
    }
}