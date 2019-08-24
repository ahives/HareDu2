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
                Console.WriteLine($"Queue: {queue.Name}");
                Console.WriteLine($"VHost: {queue.VirtualHost}");
                Console.WriteLine($"Node: {queue.Node}");
                Console.WriteLine("Churn Metrics");
                Console.WriteLine($"\tReady: {queue.Churn.Ready.Total} | {queue.Churn.Ready.Rate} msg/s");
                Console.WriteLine($"\tAcknowledged: {queue.Churn.Acknowledged.Total} | {queue.Churn.Acknowledged.Rate} msg/s");
                Console.WriteLine($"\tUnacknowledged: {queue.Churn.Unacknowledged.Total} | {queue.Churn.Unacknowledged.Rate} msg/s");
                Console.WriteLine($"\tDelivered: {queue.Churn.Delivered.Total} | {queue.Churn.Delivered.Rate} msg/s");
                Console.WriteLine($"Target Count in RAM: {queue.Internals.TargetCountOfMessagesAllowedInRAM}");
//                Console.WriteLine($"");
            }

//            Console.WriteLine();
        }
    }
}