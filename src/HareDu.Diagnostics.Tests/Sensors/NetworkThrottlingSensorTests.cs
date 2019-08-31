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
    public class NetworkThrottlingSensorTests
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
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 9, 4.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Yellow,result.Status);
        }

        [Test(Description = "When sockets used >= calculated high watermark and calculated high watermark >= max sockets available")]
        public void Verify_sensor_red_condition()
        {
            var configProvider = new DefaultConfigProvider1();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 10, 4.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Red,result.Status);
        }

        [Test]
        public void Verify_sensor_green_condition()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_1()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_condition_2()
        {
            var configProvider = new DefaultConfigProvider2();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new NetworkThrottlingSensor(configProvider, knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot1(10, 4, 4.2M);

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
                    Node = new NodeSensorConfigImpl();
                }

                public ConnectionSensorConfig Connection { get; }
                public ChannelSensorConfig Channel { get; }
                public QueueSensorConfig Queue { get; }
                public NodeSensorConfig Node { get; }

                
                class NodeSensorConfigImpl :
                    NodeSensorConfig
                {
                    public NodeSensorConfigImpl()
                    {
                        SocketUsageCoefficient = 1.0M;
                    }

                    public decimal SocketUsageCoefficient { get; }
                }
            }
        }

        
        class DefaultConfigProvider2 :
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