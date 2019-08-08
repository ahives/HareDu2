namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Alerts;
    using Core;
    using Core.Extensions;
    using Diagnostics;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionSnapshotTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
            var connection = Client
                .Snapshot<RmqBrokerConnection>()
                .Execute()
                .RunDiagnostics();
//            var snapshot = await Client
//                .Snapshot<RmqConnection>()
//                .Get();

            var data = connection
                .Select(x => x.Snapshot)
                .Select(x => x.Data);
            var connections = connection
                .Select(x => x.Snapshot)
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
                    data.Select(x => x.ConnectionsCreated).Select(x => x.Total),
                    data.Select(x => x.ConnectionsClosed).Select(x => x.Rate),
                    data.Select(x => x.ConnectionsClosed).Select(x => x.Total),
                    data.Select(x => x.ConnectionsClosed).Select(x => x.Rate));
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
                .Snapshot<RmqBrokerConnection>()
                .RunDiagnostics()
                .Execute();
//            var snapshot = await Client
//                .Snapshot<RmqConnection>()
//                .Get();
            
//            Console.WriteLine(snapshot.Select(x => x.Data).ToJsonString());
            Console.WriteLine(connection.Snapshot.ToJsonString());
        }

        [Test]
        public async Task Test3()
        {
            var connection = Client
                .Snapshot<RmqBrokerConnection>()
                .Execute()
                .RunDiagnostics();

//            var resource = Client.Snapshot<RmqConnection>();
//            var snapshot = resource.Get();
//            var data = snapshot.Select(x => x.Data);
//            var diagnosticResults = resource.RunDiagnostics(data).ToList();

            var results = connection.Select(x => x.DiagnosticResults);
            
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", results[i].Identifier, results[i].Status);
                
                if (results[i].Status == DiagnosticStatus.Red)
                {
                    Console.WriteLine(results[i].Reason);
                    Console.WriteLine(results[i].Remediation);
                }
            }
        }

        [Test]
        public async Task Test4()
        {
            var snapshot = Client
                .Snapshot<RmqBrokerConnection>()
                .RunDiagnostics()
                .Execute();
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
                .Snapshot<RmqBrokerConnection>()
                .RunDiagnostics()
                .Execute();
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