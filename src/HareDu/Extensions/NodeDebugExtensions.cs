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

    public static class NodeDebugExtensions
    {
        public static Task<ResultList<NodeInfo>> ScreenDump(this Task<ResultList<NodeInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Operating System PID: {item.OperatingSystemProcessId}");

                Console.WriteLine("Applications");
                foreach (var application in item.Applications)
                {
                    Console.WriteLine($"\tName: {application.Name}");
                    Console.WriteLine($"\tDescription: {application.Description}");
                    Console.WriteLine($"\tVersion: {application.Version}");
                    Console.WriteLine("\t-------------------");
                }

                Console.WriteLine();
                Console.WriteLine("Contexts");
                foreach (var context in item.Contexts)
                {
                    Console.WriteLine($"\tDescription: {context.Description}");
                    Console.WriteLine($"\tPath: {context.Path}");
                    Console.WriteLine($"\tPort: {context.Port}");
                    Console.WriteLine("\t-------------------");
                }

                Console.WriteLine();
                Console.WriteLine("Partitions");
                foreach (var partition in item.Partitions)
                {
                    Console.WriteLine($"\tDescription: {partition}");
                }

                Console.WriteLine();
                Console.WriteLine("Authentication Mechanisms");
                foreach (var mechanism in item.AuthenticationMechanisms)
                {
                    Console.WriteLine($"\tName: {mechanism.Name}");
                    Console.WriteLine($"\tDescription: {mechanism.Description}");
                    Console.WriteLine($"\tIsEnabled: {mechanism.IsEnabled}");
                    Console.WriteLine("\t-------------------");
                }

                Console.WriteLine();
                Console.WriteLine("Config Files");
                foreach (var file in item.ConfigFiles)
                {
                    Console.WriteLine($"\tFile: {file}");
                }

                Console.WriteLine();
                Console.WriteLine("Enabled Plugins");
                foreach (var plugin in item.EnabledPlugins)
                {
                    Console.WriteLine($"\tFile: {plugin}");
                }

                Console.WriteLine();
                Console.WriteLine("Exchange Types");
                foreach (var type in item.ExchangeTypes)
                {
                    Console.WriteLine($"\tName: {type.Name}");
                    Console.WriteLine($"\tDescription: {type.Description}");
                    Console.WriteLine($"\tIsEnabled: {type.IsEnabled}");
                    Console.WriteLine("\t-------------------");
                }

                Console.WriteLine();
                Console.WriteLine("Exchange Types");
                foreach (var file in item.LogFiles)
                {
                    Console.WriteLine($"\tFile: {file}");
                }
                
                Console.WriteLine();
                Console.WriteLine($"Type: {item.Type}");
                Console.WriteLine($"Uptime: {item.Uptime}");
                Console.WriteLine($"Context Switches: {item.ContextSwitches}");
                Console.WriteLine($"Database Directory: {item.DatabaseDirectory}");
                Console.WriteLine($"GC Details: {item.GcDetails.Rate}");
                Console.WriteLine($"Reclaimed Bytes from GC Details: {item.ReclaimedBytesFromGCDetails.Rate}");
                Console.WriteLine($"IsRunning: {item.IsRunning}");
                Console.WriteLine($"Log File: {item.LogFile}");
                
                Console.WriteLine("Processes");
                Console.WriteLine($"\tTotal: {item.TotalProcesses}");
                Console.WriteLine($"\tUsed: {item.ProcessesUsed} (total), {item.ProcessUsageDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("File Descriptors");
                Console.WriteLine($"\tTotal: {item.TotalFileDescriptors}");
                Console.WriteLine($"\tUsed: {item.FileDescriptorUsed} (total), {item.FileDescriptorUsedDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Memory");
                Console.WriteLine($"\tAlarm: {item.MemoryAlarm}");
                Console.WriteLine($"\tLimit: {item.MemoryLimit}");
                Console.WriteLine($"\tUsed: {item.MemoryUsed} (total), {item.MemoryUsageDetails.Rate} (rate)");
                Console.WriteLine($"\tCalculation Strategy: {item.MemoryCalculationStrategy}");
                Console.WriteLine();
                
                Console.WriteLine("Disk");
                Console.WriteLine($"\tAlarm: {item.FreeDiskAlarm}");
                Console.WriteLine($"\tLimit: {item.FreeDiskLimit}");
                Console.WriteLine($"\tSpace: {item.FreeDiskSpace} (total), {item.FreeDiskSpaceDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Garbage Collection Metrics");
                Console.WriteLine($"\tChannels Closed: {item.GarbageCollectionMetrics.ChannelsClosed}");
                Console.WriteLine($"\tConnections Closed: {item.GarbageCollectionMetrics.ConnectionsClosed}");
                Console.WriteLine($"\tConsumers Deleted: {item.GarbageCollectionMetrics.ConsumersDeleted}");
                Console.WriteLine($"\tExchanges Deleted: {item.GarbageCollectionMetrics.ExchangesDeleted}");
                Console.WriteLine($"\tNodes Deleted: {item.GarbageCollectionMetrics.NodesDeleted}");
                Console.WriteLine($"\tQueues Deleted: {item.GarbageCollectionMetrics.QueuesDeleted}");
                Console.WriteLine($"\tChannel Consumers Deleted: {item.GarbageCollectionMetrics.ChannelConsumersDeleted}");
                Console.WriteLine($"\tVirtual Hosts Deleted: {item.GarbageCollectionMetrics.VirtualHostsDeleted}");
                Console.WriteLine($"SASL Log File: {item.SaslLogFile}");
                Console.WriteLine();

                Console.WriteLine($"Rates Mode: {item.RatesMode}");
                Console.WriteLine($"Run Queue: {item.RunQueue}");
                Console.WriteLine($"Available Cores Detected: {item.AvailableCoresDetected}");
                Console.WriteLine($"Context Switch Details: {item.ContextSwitchDetails.Rate}");
                
                Console.WriteLine("Channels");
                Console.WriteLine($"\tCreated: {item.TotalChannelsCreated} (total), {item.CreatedChannelDetails.Rate} (rate)");
                Console.WriteLine($"\tClosed: {item.TotalChannelsClosed} (total), {item.ClosedChannelDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Connections");
                Console.WriteLine($"\tCreated: {item.TotalConnectionsCreated} (total), {item.CreatedConnectionDetails.Rate} (rate)");
                Console.WriteLine($"\tClosed: {item.TotalConnectionsClosed} (total), {item.ClosedConnectionDetails.Rate} (rate)");
                Console.WriteLine();

                Console.WriteLine("Queues");
                Console.WriteLine($"\tCreated: {item.TotalQueuesCreated} (total), {item.CreatedQueueDetails.Rate} (rate)");
                Console.WriteLine($"\tDeclared: {item.TotalQueuesDeclared} (total), {item.DeclaredQueueDetails.Rate} (rate)");
                Console.WriteLine($"\tDeleted: {item.TotalQueuesDeleted} (total), {item.DeletedQueueDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine($"Sockets Used: {item.SocketsUsed} (total), {item.TotalSocketsAvailable} (rate)");
                
                Console.WriteLine("Message Store");
                Console.WriteLine($"\tReads: {item.TotalMessageStoreReads} (total), {item.MessageStoreReadCountDetails.Rate} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalMessageStoreWrites} (total), {item.MessageStoreWriteCountDetails.Rate} (rate)");
                Console.WriteLine();

                Console.WriteLine("Mnesia Transactions");
                Console.WriteLine($"\tDisk: {item.TotalMnesiaDiskTransactions} (total), {item.MnesiaDiskTransactionCountDetails.Rate} (rate)");
                Console.WriteLine($"\tRAM: {item.TotalMnesiaRamTransactions} (total), {item.MnesiaRAMTransactionCountDetails.Rate} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Queue Index");
                Console.WriteLine($"\tReads: {item.TotalQueueIndexReads} (total), {item.QueueIndexReadCountDetails.Rate} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalQueueIndexWrites} (total), {item.QueueIndexWriteCountDetails.Rate} (rate), {item.QueueIndexJournalWriteCountDetails.Rate} (journal)");
                Console.WriteLine();

                Console.WriteLine("IO");
                Console.WriteLine($"\tReads: {item.TotalIOReads} (total), {item.IOReadDetails.Rate} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalIOWrites} (total), {item.IOWriteDetails.Rate} (rate)");
                Console.WriteLine("\tBytes");
                Console.WriteLine($"\t\tReads: {item.TotalIOBytesRead} (total), {item.IOReadBytesDetails.Rate} (rate)");
                Console.WriteLine($"\t\tWritten: {item.TotalIOBytesWritten} (total), {item.IOWriteBytesDetails.Rate} (rate)");
                Console.WriteLine($"\tSeeks: {item.IOSeekCount} (total), {item.IOSeeksDetails.Rate} (rate), {item.AvgIOSeekTimeDetails.Rate} (avg. time)");
                Console.WriteLine($"\tSyncs: {item.IOSyncCount} (total), {item.IOSyncsDetails.Rate} (rate), {item.AvgIOSyncTimeDetails.Rate} (avg. time)");
                Console.WriteLine();
                
                Console.WriteLine("File Handles");
                Console.WriteLine($"\tOpen Attempts: {item.TotalOpenFileHandleAttempts} (total), {item.FileHandleOpenAttemptCountDetails.Rate} (rate), {item.FileHandleOpenAttemptAvgTimeDetails.Rate} (avg. time)");
                Console.WriteLine();
                
                Console.WriteLine($"Total Journal Writes: {item.TotalQueueIndexJournalWrites}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }

        public static Task<Result<NodeMemoryUsageInfo>> ScreenDump(this Task<Result<NodeMemoryUsageInfo>> result)
        {
            var item = result
                .GetResult()
                .Select(x => x.Data);

            Console.WriteLine($"Atom: {item.Memory.Atom}");
            Console.WriteLine($"Binary: {item.Memory.Binary}");
            Console.WriteLine($"Metrics: {item.Memory.Metrics}");
            Console.WriteLine($"Mnesia: {item.Memory.Mnesia}");
            Console.WriteLine($"Plugins: {item.Memory.Plugins}");
            Console.WriteLine($"Strategy: {item.Memory.Strategy}");
            Console.WriteLine($"Allocated Unused: {item.Memory.AllocatedUnused}");
            Console.WriteLine($"Byte Code: {item.Memory.ByteCode}");
            Console.WriteLine($"Connection Channels: {item.Memory.ConnectionChannels}");
            Console.WriteLine($"Connection Other: {item.Memory.ConnectionOther}");
            Console.WriteLine($"Connection Readers: {item.Memory.ConnectionReaders}");
            Console.WriteLine($"Connection Writers: {item.Memory.ConnectionWriters}");
            Console.WriteLine($"Management Database: {item.Memory.ManagementDatabase}");
            Console.WriteLine($"Message Index: {item.Memory.MessageIndex}");
            Console.WriteLine($"Other Processes: {item.Memory.OtherProcesses}");
            Console.WriteLine($"Other System: {item.Memory.OtherSystem}");
            Console.WriteLine($"Queue Processes: {item.Memory.QueueProcesses}");
            Console.WriteLine($"Reserved Unallocated: {item.Memory.ReservedUnallocated}");
            Console.WriteLine($"Queue Slave Processes: {item.Memory.QueueSlaveProcesses}");
            Console.WriteLine($"Quorum Queue Processes: {item.Memory.QuorumQueueProcesses}");
            Console.WriteLine($"Other In-memory Storage: {item.Memory.OtherInMemoryStorage}");
            Console.WriteLine($"Quorum In-memory Storage: {item.Memory.QuorumInMemoryStorage}");
            Console.WriteLine("Totals");
            Console.WriteLine($"\tAllocated: {item.Memory?.Total?.Allocated}");
            Console.WriteLine($"\tErlang: {item.Memory?.Total?.Erlang}");
            Console.WriteLine($"\tStrategy: {item.Memory?.Total?.Strategy}");

            return result;
        }

        public static Task<Result<NodeHealthInfo>> ScreenDump(this Task<Result<NodeHealthInfo>> result)
        {
            var item = result
                .GetResult()
                .Select(x => x.Data);

            Console.WriteLine($"Reason: {item.Reason}");
            Console.WriteLine($"Status: {item.Status}");

            return result;
        }
    }
}