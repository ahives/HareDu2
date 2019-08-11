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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutofacIntegration;
    using NUnit.Framework;
    using Scanning;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticSensorTests :
        DiagnosticsTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
            builder.RegisterModule<HareDuDiagnosticsModule>();

            _container = builder.Build();
        }

        [Test]
        public void Test()
        {
            ConnectivitySnapshot snapshot = new FakeConnectivitySnapshot();
            var scanner = _container.Resolve<IDiagnosticScanner>();

            var report = scanner.Scan(snapshot);
            
            for (int i = 0; i < report.Results.Count; i++)
            {
                Console.WriteLine("Diagnostic => Sensor: {0}, Status: {1} [Info => {2} \"{3}\"]",
                    report.Results[i].SensorIdentifier,
                    report.Results[i].Status,
                    report.Results[i].ComponentType,
                    report.Results[i].ComponentIdentifier);
                
                if (report.Results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine("\tReason: {0}", report.Results[i].Reason);
                    Console.WriteLine("\tRemediation: {0}", report.Results[i].Remediation);
                }
            }
        }

        class FakeConnectivitySnapshot :
            ConnectivitySnapshot
        {
            public FakeConnectivitySnapshot()
            {
                Connections = GetConnections().ToList();
            }

            IEnumerable<ConnectionSnapshot> GetConnections()
            {
                yield return new FakeConnectionSnapshot("Connection1", 2);
                yield return new FakeConnectionSnapshot("Connection2", 4);
            }

            
            class FakeConnectionSnapshot :
                ConnectionSnapshot
            {
                public FakeConnectionSnapshot(string identifier, long channelLimit)
                {
                    Identifier = identifier;
                    ChannelLimit = channelLimit;
                    Channels = GetChannels().ToList();
                }

                IEnumerable<ChannelSnapshot> GetChannels()
                {
                    yield return new FakeChannelSnapshot("Channel1", 4, 2, 5, 8, 5, 1);
                    yield return new FakeChannelSnapshot("Channel2", 4, 2, 5, 8, 2, 1);
                }

                
                class FakeChannelSnapshot :
                    ChannelSnapshot
                {
                    public FakeChannelSnapshot(string name, long prefetchCount, long uncommittedAcknowledgements,
                        long uncommittedMessages, long unconfirmedMessages, long unacknowledgedMessages, long consumers)
                    {
                        PrefetchCount = prefetchCount;
                        UncommittedAcknowledgements = uncommittedAcknowledgements;
                        UncommittedMessages = uncommittedMessages;
                        UnconfirmedMessages = unconfirmedMessages;
                        UnacknowledgedMessages = unacknowledgedMessages;
                        Consumers = consumers;
                        Name = name;
                    }

                    public long PrefetchCount { get; }
                    public long UncommittedAcknowledgements { get; }
                    public long UncommittedMessages { get; }
                    public long UnconfirmedMessages { get; }
                    public long UnacknowledgedMessages { get; }
                    public long Consumers { get; }
                    public string Name { get; }
                    public string Node { get; }
                }

                public string Identifier { get; }
                public NetworkTrafficSnapshot NetworkTraffic { get; }
                public long ChannelLimit { get; }
                public string Node { get; }
                public string VirtualHost { get; }
                public IReadOnlyList<ChannelSnapshot> Channels { get; }
            }

            public ChurnMetrics ChannelsClosed { get; }
            public ChurnMetrics ChannelsCreated { get; }
            public ChurnMetrics ConnectionsClosed { get; }
            public ChurnMetrics ConnectionsCreated { get; }
            public IReadOnlyList<ConnectionSnapshot> Connections { get; }
        }
    }
}