namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterSnapshotTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
            var snapshot = await Client
                .Snapshot<RabbitMqCluster>()
                .GetDetails();

//            snapshot.Queues[0].Memory.RAM

            for (int i = 0; i < snapshot.Nodes.Count; i++)
            {
                for (int j = 0; j < snapshot.Nodes[i].Connections.Count; j++)
                {
                    Console.WriteLine();
                    Console.WriteLine("***************************Connection Details**************************************");
                    Console.WriteLine("Traffic (Sent) => Total: {0}, Octets: {1}, Rate: {2}",
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Sent.Total,
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Sent.Bytes,
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Sent.Rate);
                    Console.WriteLine("Traffic (Received) => Total: {0}, Octets: {1}, Rate: {2}",
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Received.Total,
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Received.Bytes,
                        snapshot.Nodes[i].Connections[j].NetworkTraffic.Received.Rate);
                    
                    Console.WriteLine();
                    Console.WriteLine("***************************Channel Details**************************************");
                    for (int k = 0; k < snapshot.Nodes[i].Connections[j].Channels.Count; k++)
                    {
                        Console.WriteLine("Name => {0}", snapshot.Nodes[i].Connections[j].Channels[k].Name);
                        Console.WriteLine("Consumers => {0}", snapshot.Nodes[i].Connections[j].Channels[k].TotalConsumers);
                    }
                }
                
                Console.WriteLine();
                Console.WriteLine("***************************Erlang Details**************************************");
                Console.WriteLine("Version => {0}", snapshot.Nodes[i].Erlang.Version);
                Console.WriteLine("Available CPU Cores => {0}", snapshot.Nodes[i].Erlang.AvailableCores);
                Console.WriteLine("Processes => Limit: {0}, Used: {1}, Usage Rate: {2}", snapshot.Nodes[i].Erlang.Processes.Limit, snapshot.Nodes[i].Erlang.Processes.Used, snapshot.Nodes[i].Erlang.Processes.UsageRate);
                Console.WriteLine("File Descriptors => Available: {0}, Used: {1}, Usage Rate: {2}, Open Attempts: {3}, Open Attempt Rate: {4}, Open Attempts Avg. Time: {5}, Open Attempts Avg. Time Rate: {6}", snapshot.Nodes[i].OS.FileDescriptors.Available,
                    snapshot.Nodes[i].OS.FileDescriptors.Used,
                    snapshot.Nodes[i].OS.FileDescriptors.UsageRate,
                    snapshot.Nodes[i].OS.FileDescriptors.OpenAttempts,
                    snapshot.Nodes[i].OS.FileDescriptors.OpenAttemptRate,
                    snapshot.Nodes[i].OS.FileDescriptors.OpenAttemptAvgTime,
                    snapshot.Nodes[i].OS.FileDescriptors.OpenAttemptAvgTimeRate);
//                Console.WriteLine(" => {0}", snapshot.Nodes[i]);
//                Console.WriteLine(" => {0}", snapshot.Nodes[i]);
                
            }
//            Console.WriteLine("", snapshot);
//            Console.WriteLine("Acknowledged => Total: {0}, Rate: {1}", snapshot.Queue.Acknowledged.Total, snapshot.Queue.Acknowledged.Rate);
//            Console.WriteLine("Delivered => Total: {0}, Rate: {1}", snapshot.Queue.Delivered.Total, snapshot.Queue.Delivered.Rate);
//            Console.WriteLine("Gets => Total: {0}, Rate: {1}", snapshot.Queue.Gets.Total, snapshot.Queue.Gets.Rate);
//            Console.WriteLine("Published => Total: {0}, Rate: {1}", snapshot.Queue.Published.Total, snapshot.Queue.Published.Rate);
//            Console.WriteLine("Redelivered => Total: {0}, Rate: {1}", snapshot.Queue.Redelivered.Total, snapshot.Queue.Redelivered.Rate);
//            Console.WriteLine("DeliveryGets => Total: {0}, Rate: {1}", snapshot.Queue.DeliveryGets.Total, snapshot.Queue.DeliveryGets.Rate);
//            Console.WriteLine("DeliveredWithoutAcknowledgement => Total: {0}, Rate: {1}", snapshot.Queue.DeliveredWithoutAcknowledgement.Total, snapshot.Queue.DeliveredWithoutAcknowledgement.Rate);
//            Console.WriteLine("GetsWithoutAcknowledgement => Total: {0}, Rate: {1}", snapshot.Queue.GetsWithoutAcknowledgement.Total, snapshot.Queue.GetsWithoutAcknowledgement.Rate);
//            Console.WriteLine("Disk Reads => Total: {0}, Rate: {1}", snapshot.Nodes[0].IO.Reads.Total, snapshot.Nodes[0].IO.Reads.Rate);
//            Console.WriteLine("Disk Writes => Total: {0}, Rate: {1}", snapshot.Nodes[0].IO.Writes.Total, snapshot.Nodes[0].IO.Writes.Rate);
            
//            Console.WriteLine(": {0}", summary.IOStatusDetails);
        }
    }
}