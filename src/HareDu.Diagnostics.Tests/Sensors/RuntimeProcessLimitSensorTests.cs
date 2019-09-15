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
namespace HareDu.Diagnostics.Tests.Sensors
{
    using Autofac;
    using Diagnostics.Configuration;
    using Diagnostics.Sensors;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Snapshotting.Model;

    [TestFixture]
    public class RuntimeProcessLimitSensorTests
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

        [Test(Description = "")]
        public void Verify_sensor_red_condition_1()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RuntimeProcessLimitSensor(configProvider, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 3, 3.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Red,result.Status);
            Assert.AreEqual(typeof(RuntimeProcessLimitSensor).FullName.GenerateIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test(Description = "")]
        public void Verify_sensor_red_condition_2()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RuntimeProcessLimitSensor(configProvider, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 4, 3.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Red,result.Status);
            Assert.AreEqual(typeof(RuntimeProcessLimitSensor).FullName.GenerateIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test]
        public void Verify_sensor_green_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RuntimeProcessLimitSensor(configProvider, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
            Assert.AreEqual(typeof(RuntimeProcessLimitSensor).FullName.GenerateIdentifier(), result.KnowledgeBaseArticle.Identifier);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticScannerConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RuntimeProcessLimitSensor(configProvider, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }
    }
}