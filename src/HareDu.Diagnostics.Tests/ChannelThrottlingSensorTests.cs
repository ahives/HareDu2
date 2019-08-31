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
namespace HareDu.Diagnostics.Tests
{
    using Autofac;
    using Configuration;
    using Fakes;
    using KnowledgeBase;
    using NUnit.Framework;
    using Sensors;
    using Snapshotting.Model;

    [TestFixture]
    public class ChannelThrottlingSensorTests
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
        public void Verify_sensor_red_when_unacknowledged_messages_greater_than_prefetch_count()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new ChannelThrottlingSensor(configProvider, knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot1("Channel1", 4, 2, 5, 8, 6, 1);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Red,result.Status);
        }

        [Test]
        public void Verify_sensor_green_when_unacknowledged_messages_less_than_prefetch_count()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new ChannelThrottlingSensor(configProvider, knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot1("Channel1", 6, 2, 5, 8, 4, 1);

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Green,result.Status);
        }

        [Test]
        public void Verify_sensor_inconclusive_when_snapshot_null()
        {
            var configProvider = _container.Resolve<IDiagnosticSensorConfigProvider>();
            var knowledgeBaseProvider = _container.Resolve<IKnowledgeBaseProvider>();
            var sensor = new ChannelThrottlingSensor(configProvider, knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = null;

            var result = sensor.Execute(snapshot);
            
            Assert.AreEqual(DiagnosticStatus.Inconclusive,result.Status);
        }
    }
}