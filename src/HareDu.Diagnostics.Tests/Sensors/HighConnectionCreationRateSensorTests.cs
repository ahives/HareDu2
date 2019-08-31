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
    using Configuration;
    using Diagnostics.Sensors;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Snapshotting.Model;

    [TestFixture]
    public class HighConnectionCreationRateSensorTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultKnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            builder.RegisterType<DiagnosticSensorConfigProvider>()
                .As<IDiagnosticSensorConfigProvider>()
                .SingleInstance();
            
            _container = builder.Build();
        }

        [Test]
        public void Verify_sensor_yellow_condition_1()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot4(102, 100);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Yellow,result.Status);
        }

        [Test]
        public void Verify_sensor_yellow_condition_2()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot4(100, 100);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Yellow,result.Status);
        }

        [Test]
        public void Verify_sensor_green_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot4(99, 100);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_1()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_2()
        {
            var configProvider = new DefaultConfigProvider();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot4(99, 100);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        
        class DefaultConfigProvider :
            IDiagnosticSensorConfigProvider
        {
            public bool TryGet(out DiagnosticSensorConfig config)
            {
                config = null;
                return false;
            }
        }
    }
}