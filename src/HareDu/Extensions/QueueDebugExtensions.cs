// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class QueueDebugExtensions
    {
        public static Task<ResultList<QueueInfo>> ScreenDump(this Task<ResultList<QueueInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine($"Auto Delete: {item.AutoDelete}");
                Console.WriteLine($"Consumers: {item.Consumers}");
                Console.WriteLine($"Durable: {item.Durable}");
                Console.WriteLine($"Exclusive: {item.Exclusive}");
                Console.WriteLine($"Memory: {item.Memory}");
                Console.WriteLine($"Node: {item.Node}");
                Console.WriteLine($"Policy: {item.Policy}");
                Console.WriteLine($"State: {item.State}");
                Console.WriteLine($"Consumer Utilization: {item.ConsumerUtilization}");
                Console.WriteLine();
                Console.WriteLine("Garbage Collection");
                Console.WriteLine($"\tFull Sweep After: {item.GC?.FullSweepAfter}");
                Console.WriteLine($"\tMaximum Heap Size: {item.GC?.MaximumHeapSize}");
                Console.WriteLine($"\tMinimum Heap Size: {item.GC?.MinimumHeapSize}");
                Console.WriteLine($"\tMinor Garbage Collection: {item.GC?.MinorGarbageCollection}");
                Console.WriteLine($"\tMinimum Binary Virtual Heap Size: {item.GC?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine("\t-------------------");
                Console.WriteLine();
                Console.WriteLine($"Idle Since: {item.IdleSince}");
                Console.WriteLine($"Message Rate: {item.MessageDetails?.Rate}");
                Console.WriteLine($"Messages Persisted: {item.MessagesPersisted}");
                Console.WriteLine();
                Console.WriteLine("Message Stats");
                Console.WriteLine($"\tMessage Delivery Details: {item.MessageStats?.MessageDeliveryDetails?.Rate}");
                Console.WriteLine($"\tMessage Get Details: {item.MessageStats?.MessageGetDetails?.Rate}");
                Console.WriteLine($"\tMessages Acknowledged Details: {item.MessageStats?.MessagesAcknowledgedDetails?.Rate}");
                Console.WriteLine($"\tMessages Published Details: {item.MessageStats?.MessagesPublishedDetails?.Rate}");
                Console.WriteLine($"\tMessages Redelivered Details: {item.MessageStats?.MessagesRedeliveredDetails?.Rate}");
                Console.WriteLine($"\tMessage Delivery Get Details: {item.MessageStats?.MessageDeliveryGetDetails?.Rate}");
                Console.WriteLine($"\tMessage Gets Without Ack Details: {item.MessageStats?.MessageGetsWithoutAckDetails?.Rate}");
                Console.WriteLine($"\tMessages Delivered Without Ack Details: {item.MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate}");
                Console.WriteLine($"\tTotal Message Gets: {item.MessageStats?.TotalMessageGets}");
                Console.WriteLine($"\tTotal Messages Acknowledged: {item.MessageStats?.TotalMessagesAcknowledged}");
                Console.WriteLine($"\tTotal Messages Delivered: {item.MessageStats?.TotalMessagesDelivered}");
                Console.WriteLine($"\tTotal Messages Published: {item.MessageStats?.TotalMessagesPublished}");
                Console.WriteLine($"\tTotal Messages Redelivered: {item.MessageStats?.TotalMessagesRedelivered}");
                Console.WriteLine($"\tTotal Message Delivery Gets: {item.MessageStats?.TotalMessageDeliveryGets}");
                Console.WriteLine($"\tTotal Message Delivered Without Ack: {item.MessageStats?.TotalMessageDeliveredWithoutAck}");
                Console.WriteLine($"\tTotal Message Gets Without Ack: {item.MessageStats?.TotalMessageGetsWithoutAck}");
                Console.WriteLine("\t-------------------");
                Console.WriteLine();
                Console.WriteLine($"Ready Messages: {item.ReadyMessages}");
                Console.WriteLine($"Reduction Rate: {item.ReductionDetails?.Rate}");
                Console.WriteLine($"Total Messages: {item.TotalMessages}");
                Console.WriteLine($"Total Reductions: {item.TotalReductions}");
                Console.WriteLine($"Unacknowledged Messages: {item.UnacknowledgedMessages}");
                Console.WriteLine("Backing Queue Status");
                Console.WriteLine($"\tLength: {item.BackingQueueStatus?.Length}");
                Console.WriteLine($"\tMode: {item.BackingQueueStatus?.Mode}");
                Console.WriteLine($"\tQ1: {item.BackingQueueStatus?.Q1}");
                Console.WriteLine($"\tQ2: {item.BackingQueueStatus?.Q2}");
                Console.WriteLine($"\tQ3: {item.BackingQueueStatus?.Q3}");
                Console.WriteLine($"\tQ4: {item.BackingQueueStatus?.Q4}");
                Console.WriteLine($"\tNext Sequence Id: {item.BackingQueueStatus?.NextSequenceId}");
                Console.WriteLine($"\tAvg. Egress Rate: {item.BackingQueueStatus?.AvgEgressRate}");
                Console.WriteLine($"\tAvg. Ingress Rate: {item.BackingQueueStatus?.AvgIngressRate}");
                Console.WriteLine($"\tAvg. Acknowledgement Egress Rate: {item.BackingQueueStatus?.AvgAcknowledgementEgressRate}");
                Console.WriteLine($"\tAvg. Acknowledgement Ingress Rate: {item.BackingQueueStatus?.AvgAcknowledgementIngressRate}");
                Console.WriteLine($"\tTarget Total Messages in RAM: {item.BackingQueueStatus?.TargetTotalMessagesInRAM}");
                Console.WriteLine($"Exclusive Consumer Tag: {item.ExclusiveConsumerTag}");
                Console.WriteLine($"Head Message Timestamp: {item.HeadMessageTimestamp}");
                Console.WriteLine($"Message Bytes Persisted: {item.MessageBytesPersisted}");
                Console.WriteLine($"Messages (RAM): {item.MessagesInRAM}");
                Console.WriteLine($"Ready Message Rate: {item.ReadyMessageDetails?.Rate}");
                Console.WriteLine($"Unacked Message Rate: {item.UnacknowledgedMessageDetails?.Rate}");
                Console.WriteLine($"Message Bytes (RAM): {item.MessageBytesInRAM}");
                Console.WriteLine($"Message Bytes Paged Out: {item.MessageBytesPagedOut}");
                Console.WriteLine($"Total Messages Paged Out: {item.TotalMessagesPagedOut}");
                Console.WriteLine($"UnacknowledgedMessages (RAM): {item.UnacknowledgedMessagesInRAM}");
                Console.WriteLine($"Total Bytes Of All Messages: {item.TotalBytesOfAllMessages}");
                Console.WriteLine($"Messages Ready for Delivery (RAM): {item.MessagesReadyForDeliveryInRAM}");
                Console.WriteLine($"Total Bytes (Messages Delivered - Unacknowledged): {item.TotalBytesOfMessagesDeliveredButUnacknowledged}");
                Console.WriteLine($"Total Bytes (Messages Ready for Delivery): {item.TotalBytesOfMessagesReadyForDelivery}");
                Console.WriteLine("-------------------");
                Console.WriteLine();
            }

            return result;
        }
    }
}