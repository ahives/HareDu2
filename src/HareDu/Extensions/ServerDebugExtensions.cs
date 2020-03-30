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

    public static class ServerDebugExtensions
    {
        public static Task<Result<ServerInfo>> ScreenDump(this Task<Result<ServerInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            Console.WriteLine($"RabbitMqVersion: {results.RabbitMqVersion}");

            Console.WriteLine("Bindings");
            foreach (var binding in results.Bindings)
            {
                Console.WriteLine($"\tDestination: {binding.Destination}");
                Console.WriteLine($"\tDestinationType: {binding.DestinationType}");
                Console.WriteLine($"\tPropertiesKey: {binding.PropertiesKey}");
                Console.WriteLine($"\tRoutingKey: {binding.RoutingKey}");
                Console.WriteLine($"\tSource: {binding.Source}");
                Console.WriteLine($"\tVirtualHost: {binding.VirtualHost}");
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine();

            Console.WriteLine("Exchanges");
            foreach (var exchange in results.Exchanges)
            {
                Console.WriteLine($"\tAuto Delete: {exchange.AutoDelete}");
                Console.WriteLine($"\tDurable: {exchange.Durable}");
                Console.WriteLine($"\tInternal: {exchange.Internal}");
                Console.WriteLine($"\tName: {exchange.Name}");
                Console.WriteLine($"\tRoutingType: {exchange.RoutingType}");
                Console.WriteLine($"\tVirtual Host: {exchange.VirtualHost}");
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine();

            Console.WriteLine("Queues");
            foreach (var queue in results.Queues)
            {
                Console.WriteLine($"\tAuto Delete: {queue.AutoDelete}");
                Console.WriteLine($"\tDurable: {queue.Durable}");
                Console.WriteLine($"\tInternal: {queue.Consumers}");
                Console.WriteLine($"\tName: {queue.Name}");
                Console.WriteLine($"\tConsumer Utilization: {queue.ConsumerUtilization}");
                Console.WriteLine($"\tExclusive: {queue.Exclusive}");
                Console.WriteLine($"\tIdle Since: {queue.IdleSince.ToString()}");
                Console.WriteLine($"\tHead Message Timestamp: {queue.HeadMessageTimestamp.ToString()}");
                Console.WriteLine("Garbage Collection");
                Console.WriteLine($"\tMinor: {queue.GC?.MinorGarbageCollection}");
                Console.WriteLine($"\tHeap Size: {queue.GC?.MinimumHeapSize} (minimum), {queue.GC?.MaximumHeapSize} (maximum)");
                Console.WriteLine($"\tMinimum Binary Virtual Heap Size: {queue.GC?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine($"\tFull Sweep After: {queue.GC?.FullSweepAfter}");
                Console.WriteLine($"\tExclusive Consumer Tag: {queue.ExclusiveConsumerTag}");
                Console.WriteLine($"\tConsumer Utilization: {queue.ConsumerUtilization}");
                Console.WriteLine($"\tVirtual Host: {queue.VirtualHost}");
                Console.WriteLine($"\tUnacknowledged - Delivered: {queue.UnacknowledgedMessages} (total), {queue.UnacknowledgedMessagesInRAM} (RAM)");
                Console.WriteLine($"\tPaged Out: {queue.TotalMessagesPagedOut} (total), {queue.MessageBytesPagedOut} (bytes)");
                Console.WriteLine($"\tMessages Ready for Delivery: {queue.TotalBytesOfMessagesReadyForDelivery} (bytes)");
                Console.WriteLine($"\tMessages Delivered - Unacknowledged: {queue.TotalBytesOfMessagesDeliveredButUnacknowledged} (bytes)");
                Console.WriteLine($"\tMessages (bytes): {queue.TotalBytesOfAllMessages}");
                Console.WriteLine($"\tState: {queue.State}");
                Console.WriteLine($"\tRecoverable Slaves: {queue.RecoverableSlaves}");
                Console.WriteLine($"\tReductions: {queue.ReductionRate?.Rate} (rate)");
                Console.WriteLine($"\tMessages Unacknowledged: {queue.UnackedMessageRate?.Rate} (rate)");
                Console.WriteLine($"\tMessages Ready: {queue.ReadyMessageRate?.Rate} (rate)");
                Console.WriteLine($"\tMessage: {queue.MessageRate?.Rate} (rate)");
                Console.WriteLine($"\tPolicy: {queue.Policy}");
                Console.WriteLine($"\tTotal Target RAM: {queue.BackingQueueStatus?.TargetTotalMessagesInRAM}");
                Console.WriteLine($"\tQ4: {queue.BackingQueueStatus?.Q4}");
                Console.WriteLine($"\tQ3: {queue.BackingQueueStatus?.Q3}");
                Console.WriteLine($"\tQ2: {queue.BackingQueueStatus?.Q2}");
                Console.WriteLine($"\tQ1: {queue.BackingQueueStatus?.Q1}");
                Console.WriteLine($"\tNext Sequence Id: {queue.BackingQueueStatus?.NextSequenceId}");
                Console.WriteLine($"\tMode: {queue.BackingQueueStatus?.Mode}");
                Console.WriteLine($"\tLength: {queue.BackingQueueStatus?.Length}");
                Console.WriteLine($"\tAvg. Ingress (rate): {queue.BackingQueueStatus?.AvgIngressRate}");
                Console.WriteLine($"\tAvg. Egress (rate): {queue.BackingQueueStatus?.AvgEgressRate}");
                Console.WriteLine($"\tAvg. Acknowledgement Ingress (rate): {queue.BackingQueueStatus?.AvgAcknowledgementIngressRate}");
                Console.WriteLine($"\tAvg. Acknowledgement Egress (rate): {queue.BackingQueueStatus?.AvgAcknowledgementEgressRate}");
                Console.WriteLine($"\tMessage Bytes Persisted: {queue.MessageBytesPersisted}");
                Console.WriteLine($"\tMessage Bytes (RAM): {queue.MessageBytesInRAM}");
                Console.WriteLine($"\tMemory: {queue.Memory}");
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine();

            Console.WriteLine("Users");
            foreach (var user in results.Users)
            {
                Console.WriteLine($"\tHashing Algorithm: {user.HashingAlgorithm}");
                Console.WriteLine($"\tPassword Hash: {user.PasswordHash}");
                Console.WriteLine($"\tTags: {user.Tags}");
                Console.WriteLine($"\tUsername: {user.Username}");
                Console.WriteLine("\t-------------------");
            }

            foreach (var virtualHost in results.VirtualHosts)
            {
                Console.WriteLine($"\tName: {virtualHost.Name}");
                Console.WriteLine($"\tTracing: {virtualHost.Tracing}");
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine("****************************************************");
            Console.WriteLine();

            return result;
        }
    }
}