namespace HareDu.Snapshotting.IntegrationTests.Observers
{
    using System;
    using Snapshotting;
    using Model;

    public class DefaultQueueSnapshotConsoleLogger :
        IObserver<SnapshotContext<BrokerQueuesSnapshot>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SnapshotContext<BrokerQueuesSnapshot> value)
        {
//            Console.WriteLine($"");

            foreach (var queue in value.Snapshot.Queues)
            {
                Console.WriteLine($"Queue: {queue.Identifier}");
                Console.WriteLine($"VHost: {queue.VirtualHost}");
                Console.WriteLine($"Node: {queue.Node}");
                Console.WriteLine("Churn Metrics");
                Console.WriteLine($"\tReady: {queue.Messages?.Ready?.Total ?? 0} | {queue.Messages?.Ready?.Rate ?? 0} msg/s");
                Console.WriteLine($"\tAcknowledged: {queue.Messages?.Acknowledged?.Total ?? 0} | {queue.Messages?.Acknowledged?.Rate ?? 0} msg/s");
                Console.WriteLine($"\tUnacknowledged: {queue.Messages?.Unacknowledged?.Total ?? 0} | {queue.Messages?.Unacknowledged?.Rate ?? 0} msg/s");
                Console.WriteLine($"\tDelivered: {queue.Messages.Delivered.Total} | {queue.Messages?.Delivered?.Rate ?? 0} msg/s");
                Console.WriteLine($"Target Count in RAM: {queue.Internals?.TargetCountOfMessagesAllowedInRAM ?? 0}");
//                Console.WriteLine($"");
            }

//            Console.WriteLine();
        }
    }
}