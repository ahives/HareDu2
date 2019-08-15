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
namespace HareDu.Snapshotting.Tests
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionSnapshotTests :
        SnapshotTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
//            builder.RegisterModule<MassTransitModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var connection = Client
                .Snapshot<ConnectivitySnapshot, BrokerConnection>()
                .Take();

            var connections = connection
                .Select(x => x.Data)
                .Select(x => x.Connections);
            
            for (int i = 0; i < connections.Count; i++)
            {
//                snapshot.Select(x => x.Connections)[0].Select(x => x.Identifier)
                Console.WriteLine("Connection => {0}", connections[0].Select(x => x.Identifier));
//                Console.WriteLine("Connection => {0}", snapshot.Connections[i].Identifier);
                Console.WriteLine("Channel Limit => {0}", connections[i].ChannelLimit);
                Console.WriteLine("Channels => {0}", connections[i].Channels.Count);
                Console.WriteLine("Connections => {0} created | {1:0.0}/s, {2} closed | {3:0.0}/s",
                    connection.Select(x => x.Data).Select(x => x.ConnectionsCreated).Select(x => x.Total),
                    connection.Select(x => x.Data).Select(x => x.ConnectionsClosed).Select(x => x.Rate),
                    connection.Select(x => x.Data).Select(x => x.ConnectionsClosed).Select(x => x.Total),
                    connection.Select(x => x.Data).Select(x => x.ConnectionsClosed).Select(x => x.Rate));
//                Console.WriteLine("=> {0} created, {1} closed", snapshot.Select(x => x.ConnectionsCreated), snapshot.Select(x => x.ConnectionsClosed));
//                Console.WriteLine("=> {0} created, {1} closed", snapshot.Select(x => x.ConnectionsCreated), snapshot.Select(x => x.ConnectionsClosed));
                Console.WriteLine("Network Traffic");
                Console.WriteLine("\tSent: {0} packets | {1} | {2} msg/s",
                    connections[i].NetworkTraffic.Sent.Total,
                    $"{connections[i].NetworkTraffic.Sent.Bytes} bytes ({Format(connections[i].NetworkTraffic.Sent.Bytes)})",
                    connections[i].NetworkTraffic.Sent.Rate);
                Console.WriteLine("\tReceived: {0} packets | {1} | {2} msg/s",
                    connections[i].NetworkTraffic.Received.Total,
                    $"{connections[i].NetworkTraffic.Received.Bytes} bytes ({Format(connections[i].NetworkTraffic.Received.Bytes)})",
                    connections[i].NetworkTraffic.Received.Rate);

                Console.WriteLine("Channels");
                for (int j = 0; j < connections[i].Channels.Count; j++)
                {
                    Console.WriteLine("\tChannel => {0}, Consumers => {1}",
                        connections[i].Channels[j].Name,
                        connections[i].Channels[j].Consumers);
                }
                
                Console.WriteLine("****************************");
                Console.WriteLine();
//                Console.WriteLine("=> {0}", snapshot.Connections[i]);
            }
        }

        [Test]
        public async Task Test2()
        {
            var connection = Client
                .Snapshot<ConnectivitySnapshot, BrokerConnection>()
                .Take();
            
            Console.WriteLine(connection.ToJsonString());
        }

        [Test]
        public async Task Test4()
        {
            var connection = Client
                .Snapshot<ConnectivitySnapshot, BrokerConnection>()
                .Take();

//            var resource = Client.Snapshot<RmqConnection>();
//            var snapshot = resource.Get();
//            var data = snapshot.Select(x => x.Data);
//            var diagnosticResults = resource.RunDiagnostics(data).ToList();

//            for (int i = 0; i < diagnosticResults.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", diagnosticResults[i].Identifier, diagnosticResults[i].Status);
//            }
        }

        [Test]
        public async Task Test5()
        {
            var snapshot = Client
                .Snapshot<ConnectivitySnapshot, BrokerConnection>()
                .Take();

//            var snapshot = resource.Get();
//            var data = snapshot.Select(x => x.Data);
//            var diagnosticResults = resource.RunDiagnostics(data).ToList();

//            for (int i = 0; i < diagnosticResults.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", diagnosticResults[i].Identifier, diagnosticResults[i].Status);
//            }
        }

        string Format(long bytes)
        {
            if (bytes < 1000f)
                return $"{bytes}";

            if (bytes / 1000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} KB", bytes / 1000f);

            if (bytes / 1000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} MB", bytes / 1000000f);
            
            if (bytes / 1000000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} GB", bytes / 1000000000f);
            
            return $"{bytes}";
        }
    }
}