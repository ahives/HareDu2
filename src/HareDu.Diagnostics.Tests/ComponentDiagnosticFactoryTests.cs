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
    using System.Collections.Generic;
    using Diagnostics.Configuration;
    using Diagnostics.Sensors;
    using KnowledgeBase;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting.Model;

    [TestFixture]
    public class ComponentDiagnosticFactoryTests
    {
        IReadOnlyList<IDiagnosticSensor> _sensors;

        [OneTimeSetUp]
        public void Init()
        {
            var configProvider = new DiagnosticScannerConfigProvider();
            var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            
            _sensors = new List<IDiagnosticSensor>
            {
                new HighConnectionCreationRateSensor(configProvider, knowledgeBaseProvider),
                new HighConnectionClosureRateSensor(configProvider, knowledgeBaseProvider),
                new UnlimitedPrefetchCountSensor(configProvider, knowledgeBaseProvider),
                new ChannelThrottlingSensor(configProvider, knowledgeBaseProvider),
                new ChannelLimitReachedSensor(configProvider, knowledgeBaseProvider),
                new BlockedConnectionSensor(configProvider, knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_can_get_diagnostic()
        {
            var factory = new ComponentDiagnosticFactory(_sensors);
            
            Assert.IsTrue(factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic));
            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            var factory = new ComponentDiagnosticFactory(_sensors);
            
            Assert.IsFalse(factory.TryGet<ConnectionSnapshot>(out var diagnostic));
            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }
    }
}