namespace HareDu.Tests
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionSnapshotTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
//            Result<>
            var snapshot = await Client
                .Snapshot<RmqConnection>()
                .Get();

            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                Console.WriteLine("Connection => {0}", snapshot.Connections[i].Identifier);
                Console.WriteLine("Network Traffic");
                Console.WriteLine("\tSent: {0} packets | {1} | {2} msg/s",
                    snapshot.Connections[i].NetworkTraffic.Sent.Total,
                    $"{snapshot.Connections[i].NetworkTraffic.Sent.Bytes} bytes ({Format(snapshot.Connections[i].NetworkTraffic.Sent.Bytes)})",
                    snapshot.Connections[i].NetworkTraffic.Sent.Rate);
                Console.WriteLine("\tReceived: {0} packets | {1} | {2} msg/s",
                    snapshot.Connections[i].NetworkTraffic.Received.Total,
                    $"{snapshot.Connections[i].NetworkTraffic.Received.Bytes} bytes ({Format(snapshot.Connections[i].NetworkTraffic.Received.Bytes)})",
                    snapshot.Connections[i].NetworkTraffic.Received.Rate);

                Console.WriteLine("Channels");
                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    Console.WriteLine("\tChannel => {0}, Consumers => {1}",
                        snapshot.Connections[i].Channels[j].Name,
                        snapshot.Connections[i].Channels[j].TotalConsumers);
                }
                
                Console.WriteLine("****************************");
                Console.WriteLine();
//                Console.WriteLine("=> {0}", snapshot.Connections[i]);
            }
        }

        string Format(long bytes)
        {
            if (bytes / 1000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.00} KB", bytes / 1000f);

            if (bytes / 1000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.00} MB", bytes / 1000000f);
            
            if (bytes / 1000000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.00} GB", bytes / 1000000000f);
            
            return $"{bytes}";
        }
    }
}