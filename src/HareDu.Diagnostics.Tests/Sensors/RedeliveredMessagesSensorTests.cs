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
    public class RedeliveredMessagesSensorTests
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
        public void Verify_sensor_yellow_condition()
        {
            var configProvider = new DefaultConfigProvider1();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RedeliveredMessagesSensor(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 90, 32.3M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Yellow,result.Status);
        }

        [Test]
        public void Verify_sensor_green_condition()
        {
            var configProvider = new DefaultConfigProvider1();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RedeliveredMessagesSensor(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(100, 54.4M, 50, 32.3M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_1()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RedeliveredMessagesSensor(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_2()
        {
            var configProvider = new DefaultConfigProvider();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new RedeliveredMessagesSensor(configProvider, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot2(99, 54.4M, 100, 32.3M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        
        class DefaultConfigProvider1 :
            IDiagnosticSensorConfigProvider
        {
            public bool TryGet(out DiagnosticSensorConfig config)
            {
                config = new FakeDiagnosticSensorConfig();
                return true;
            }

            
            class FakeDiagnosticSensorConfig :
                DiagnosticSensorConfig
            {
                public FakeDiagnosticSensorConfig()
                {
                    SocketUsageCoefficient = 1.0M;
                    MessageRedeliveryCoefficient = 0.8M;
                }

                public int HighClosureRateThreshold { get; }
                public int HighCreationRateThreshold { get; }
                public decimal MessageRedeliveryCoefficient { get; }
                public decimal SocketUsageCoefficient { get; }
                public decimal RuntimeProcessUsageCoefficient { get; }
            }
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