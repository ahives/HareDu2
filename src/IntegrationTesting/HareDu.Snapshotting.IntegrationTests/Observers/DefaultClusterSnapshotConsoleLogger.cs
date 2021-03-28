namespace HareDu.Snapshotting.IntegrationTests.Observers
{
    using System;
    using Snapshotting;
    using Extensions;
    using Model;

    public class DefaultClusterSnapshotConsoleLogger :
        IObserver<SnapshotContext<ClusterSnapshot>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SnapshotContext<ClusterSnapshot> value)
        {
            Console.WriteLine($"Cluster Name: {value.Snapshot.ClusterName}");
            Console.WriteLine();

            for (int i = 0; i < value.Snapshot.Nodes.Count; i++)
            {
                Console.WriteLine($"Node: {value.Snapshot.Nodes[i].Identifier}");
                Console.WriteLine();
                Console.WriteLine("*************Memory*************");
                Console.WriteLine($"Limit: {value.Snapshot.Nodes[i].Memory.Limit}");
                Console.WriteLine($"Used: {value.Snapshot.Nodes[i].Memory.Used}");
                Console.WriteLine($"UsageRate: {value.Snapshot.Nodes[i].Memory.UsageRate}");
                Console.WriteLine($"AlarmInEffect: {value.Snapshot.Nodes[i].Memory.AlarmInEffect}");
                Console.WriteLine();
                Console.WriteLine("*************Disk*************");
                Console.WriteLine($"Limit: {value.Snapshot.Nodes[i].Disk.Limit}");
                Console.WriteLine($"Available: {value.Snapshot.Nodes[i].Disk.Capacity.Available}");
                Console.WriteLine($"Rate: {value.Snapshot.Nodes[i].Disk.Capacity.Rate}");
                Console.WriteLine($"AlarmInEffect: {value.Snapshot.Nodes[i].Disk.AlarmInEffect}");
                Console.WriteLine();
                Console.WriteLine("*************IO*************");
                Console.WriteLine($"Disk Reads: {value.Snapshot.Nodes[i].Disk.IO.Reads.Total}");
                Console.WriteLine($"Bytes Read: {value.Snapshot.Nodes[i].Disk.IO.Reads.Bytes.Total.ToByteString()}");
                Console.WriteLine($"Disk Read Rate: {value.Snapshot.Nodes[i].Disk.IO.Reads.Rate}");
                Console.WriteLine();
                Console.WriteLine("*************OS*************");
                Console.WriteLine($"ProcessId: {value.Snapshot.Nodes[i].OS.ProcessId}");
                Console.WriteLine($"Available: {value.Snapshot.Nodes[i].OS.FileDescriptors.Available}");
                Console.WriteLine($"Used: {value.Snapshot.Nodes[i].OS.FileDescriptors.Used}");
                Console.WriteLine($"UsageRate: {value.Snapshot.Nodes[i].OS.FileDescriptors.UsageRate}");
                Console.WriteLine($"OpenAttempts: {value.Snapshot.Nodes[i].OS.FileDescriptors.OpenAttempts}");
                Console.WriteLine($"OpenAttemptRate: {value.Snapshot.Nodes[i].OS.FileDescriptors.OpenAttemptRate}");
                Console.WriteLine($"AvgTimePerOpenAttempt: {value.Snapshot.Nodes[i].OS.FileDescriptors.AvgTimePerOpenAttempt}");
                Console.WriteLine($"AvgTimeRatePerOpenAttempt: {value.Snapshot.Nodes[i].OS.FileDescriptors.AvgTimeRatePerOpenAttempt}");
                Console.WriteLine();
                Console.WriteLine("*************Erlang Runtime*************");
                Console.WriteLine($"Identifier: {value.Snapshot.Nodes[i].Runtime.Identifier}");
                Console.WriteLine($"Version: {value.Snapshot.Nodes[i].Runtime.Version}");
                Console.WriteLine($"Processes.Limit: {value.Snapshot.Nodes[i].Runtime.Processes.Limit}");
                Console.WriteLine($"Processes.Used: {value.Snapshot.Nodes[i].Runtime.Processes.Used}");
                Console.WriteLine($"Processes.UsageRate: {value.Snapshot.Nodes[i].Runtime.Processes.UsageRate}");
            }
        }
    }
}