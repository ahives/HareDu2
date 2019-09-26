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
namespace HareDu.Snapshotting.Observers
{
    using System;
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
                Console.WriteLine($"Node: {queue.NodeIdentifier}");
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