namespace HareDu.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ServerAdminDebuggingExtensions
    {
        public static Task<ResultList<ChannelInfo>> ScreenDump(this Task<ResultList<ChannelInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Authentication Mechanism: {item.AuthenticationMechanism}");
                Console.WriteLine($"Confirm: {item.Confirm}");
                Console.WriteLine($"Connected At: {item.ConnectedAt}");
                Console.WriteLine("Connection");
                Console.WriteLine($"\tName: {item.ConnectionDetails?.Name}");
                Console.WriteLine($"\tPeer Host: {item.ConnectionDetails?.PeerHost}");
                Console.WriteLine($"\tPeer Port: {item.ConnectionDetails?.PeerPort}");
                Console.WriteLine($"Frame Max: {item.FrameMax}");
                Console.WriteLine("Garbage Collection");
                Console.WriteLine($"Full Sweep After: {item.GarbageCollectionDetails?.FullSweepAfter}");
                Console.WriteLine($"Minimum Heap Size: {item.GarbageCollectionDetails?.MinimumHeapSize}");
                Console.WriteLine($"Maximum Heap Size: {item.GarbageCollectionDetails?.MaximumHeapSize}");
                Console.WriteLine($"Minimum Binary Virtual Heap Size: {item.GarbageCollectionDetails?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine($"Minor: {item.GarbageCollectionDetails?.MinorGarbageCollection}");
                Console.WriteLine($"Global Prefetch Count: {item.GlobalPrefetchCount}");
                Console.WriteLine($"Host: {item.Host}");
                Console.WriteLine($"Idle Since: {item.IdleSince.ToString()}");
                Console.WriteLine($"Node: {item.Node}");
                Console.WriteLine($"Number: {item.Number}");
                Console.WriteLine("Peer Certificate");
                Console.WriteLine($"\tIssuer: {item.PeerCertificateIssuer}");
                Console.WriteLine($"\tSubject: {item.PeerCertificateSubject}");
                Console.WriteLine($"\tValidity: {item.PeerCertificateValidity}");
                Console.WriteLine($"Peer Host: {item.PeerHost}");
                Console.WriteLine($"Peer Port: {item.PeerPort}");
                Console.WriteLine($"Port: {item.Port}");
                Console.WriteLine($"Prefetch Count: {item.PrefetchCount}");
                Console.WriteLine($"Protocol: {item.Protocol}");
                Console.WriteLine($"Reduction: {item.ReductionDetails?.Value} (rate)");
                Console.WriteLine($"Sent Pending: {item.SentPending}");
                Console.WriteLine("SSL");
                Console.WriteLine($"\tSSL: {item.Ssl}");
                Console.WriteLine($"\tCipher: {item.SslCipher}");
                Console.WriteLine($"\tHash: {item.SslHash}");
                Console.WriteLine($"\tKey Exchange: {item.SslKeyExchange}");
                Console.WriteLine($"\tProtocol: {item.SslProtocol}");
                Console.WriteLine($"State: {item.State}");
                Console.WriteLine($"Timeout: {item.Timeout}");
                Console.WriteLine($"Total Channels: {item.TotalChannels}");
                Console.WriteLine($"Total Consumers: {item.TotalConsumers}");
                Console.WriteLine($"Total Reductions: {item.TotalReductions}");
                Console.WriteLine($"Transactional: {item.Transactional}");
                Console.WriteLine($"Type: {item.Type}");
                Console.WriteLine($"Unacknowledged Messages: {item.UnacknowledgedMessages}");
                Console.WriteLine($"Uncommitted Acknowledgements: {item.UncommittedAcknowledgements}");
                Console.WriteLine($"Uncommitted Messages: {item.UncommittedMessages}");
                Console.WriteLine($"Unconfirmed Messages: {item.UnconfirmedMessages}");
                Console.WriteLine($"User: {item.User}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }
        
        public static Task<ResultList<ConnectionInfo>> ScreenDump(this Task<ResultList<ConnectionInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Channels: {item.Channels}");
                Console.WriteLine($"Authentication Mechanism: {item.AuthenticationMechanism}");
                Console.WriteLine($"Connected At: {item.ConnectedAt}");
                Console.WriteLine($"Connection Timeout: {item.ConnectionTimeout}");
                Console.WriteLine();
                Console.WriteLine("Garbage Collection");
                Console.WriteLine($"Full Sweep After: {item.GarbageCollectionDetails?.FullSweepAfter}");
                Console.WriteLine($"Minimum Heap Size: {item.GarbageCollectionDetails?.MinimumHeapSize}");
                Console.WriteLine($"Maximum Heap Size: {item.GarbageCollectionDetails?.MaximumHeapSize}");
                Console.WriteLine($"Minimum Binary Virtual Heap Size: {item.GarbageCollectionDetails?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine($"Minor: {item.GarbageCollectionDetails?.MinorGarbageCollection}");
                Console.WriteLine($"Host: {item.Host}");
                Console.WriteLine($"Max Channels: {item.OpenChannelsLimit}");
                Console.WriteLine($"Max Frame Size (bytes): {item.MaxFrameSizeInBytes}");
                Console.WriteLine($"Bytes Received: {item.PacketBytesReceived}");
                Console.WriteLine($"Packets Received: {item.PacketsReceived}");
                Console.WriteLine($"Peer Certificate Issuer: {item.PeerCertificateIssuer}");
                Console.WriteLine($"Peer Certificate Subject: {item.PeerCertificateSubject}");
                Console.WriteLine($"Peer Host: {item.PeerHost}");
                Console.WriteLine($"Peer Port: {item.PeerPort}");
                Console.WriteLine($"Port: {item.Port}");
                Console.WriteLine($"Octets Received (rate): {item.PacketBytesReceivedDetails?.Value}");
                Console.WriteLine($"Octets Sent (rate): {item.PacketBytesSentDetails?.Value}");
                Console.WriteLine($"Send Pending: {item.SendPending}");
                Console.WriteLine("SSL");
                Console.WriteLine($"\tIs Secure: {item.IsSsl}");
                Console.WriteLine($"\tCipher Algorithm: {item.SslCipherAlgorithm}");
                Console.WriteLine($"\tHash Function: {item.SslHashFunction}");
                Console.WriteLine($"\tKey Exchange Algorithm: {item.SslKeyExchangeAlgorithm}");
                Console.WriteLine($"\tProtocol: {item.SslProtocol}");
                Console.WriteLine($"State: {item.State}");
                Console.WriteLine($"Time Period Peer Certificate Valid: {item.TimePeriodPeerCertificateValid}");
                Console.WriteLine($"Reductions: {item.TotalReductions} (total), {item.ReductionDetails?.Value} (rate)");
                Console.WriteLine($"Type: {item.Type}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }
        
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
                    Console.WriteLine($"\tDescription: {partition}");

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
                    Console.WriteLine($"\tFile: {file}");

                Console.WriteLine();
                Console.WriteLine("Enabled Plugins");
                foreach (var plugin in item.EnabledPlugins)
                    Console.WriteLine($"\tFile: {plugin}");

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
                    Console.WriteLine($"\tFile: {file}");
                
                Console.WriteLine();
                Console.WriteLine($"Type: {item.Type}");
                Console.WriteLine($"Uptime: {item.Uptime}");
                Console.WriteLine($"Context Switches: {item.ContextSwitches}");
                Console.WriteLine($"Database Directory: {item.DatabaseDirectory}");
                Console.WriteLine($"GC Details: {item.GcDetails?.Value}");
                Console.WriteLine($"Reclaimed Bytes from GC Details: {item.ReclaimedBytesFromGCDetails?.Value}");
                Console.WriteLine($"IsRunning: {item.IsRunning}");
                Console.WriteLine($"Log File: {item.LogFile}");
                
                Console.WriteLine("Processes");
                Console.WriteLine($"\tTotal: {item.TotalProcesses}");
                Console.WriteLine($"\tUsed: {item.ProcessesUsed} (total), {item.ProcessUsageDetails?.Value} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("File Descriptors");
                Console.WriteLine($"\tTotal: {item.TotalFileDescriptors}");
                Console.WriteLine($"\tUsed: {item.FileDescriptorUsed} (total), {item.FileDescriptorUsedDetails?.Value} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Memory");
                Console.WriteLine($"\tAlarm: {item.MemoryAlarm}");
                Console.WriteLine($"\tLimit: {item.MemoryLimit}");
                Console.WriteLine($"\tUsed: {item.MemoryUsed} (total), {item.MemoryUsageDetails?.Value} (rate)");
                Console.WriteLine($"\tCalculation Strategy: {item.MemoryCalculationStrategy}");
                Console.WriteLine();
                
                Console.WriteLine("Disk");
                Console.WriteLine($"\tAlarm: {item.FreeDiskAlarm}");
                Console.WriteLine($"\tLimit: {item.FreeDiskLimit}");
                Console.WriteLine($"\tSpace: {item.FreeDiskSpace} (total), {item.FreeDiskSpaceDetails?.Value} (rate)");
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
                Console.WriteLine($"Context Switch Details: {item.ContextSwitchDetails?.Value}");
                
                Console.WriteLine("Channels");
                Console.WriteLine($"\tCreated: {item.TotalChannelsCreated} (total), {item.CreatedChannelDetails?.Value} (rate)");
                Console.WriteLine($"\tClosed: {item.TotalChannelsClosed} (total), {item.ClosedChannelDetails?.Value} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Connections");
                Console.WriteLine($"\tCreated: {item.TotalConnectionsCreated} (total), {item.CreatedConnectionDetails?.Value} (rate)");
                Console.WriteLine($"\tClosed: {item.TotalConnectionsClosed} (total), {item.ClosedConnectionDetails?.Value} (rate)");
                Console.WriteLine();

                Console.WriteLine("Queues");
                Console.WriteLine($"\tCreated: {item.TotalQueuesCreated} (total), {item.CreatedQueueDetails?.Value} (rate)");
                Console.WriteLine($"\tDeclared: {item.TotalQueuesDeclared} (total), {item.DeclaredQueueDetails?.Value} (rate)");
                Console.WriteLine($"\tDeleted: {item.TotalQueuesDeleted} (total), {item.DeletedQueueDetails?.Value} (rate)");
                Console.WriteLine();
                
                Console.WriteLine($"Sockets Used: {item.SocketsUsed} (total), {item.TotalSocketsAvailable} (rate)");
                
                Console.WriteLine("Message Store");
                Console.WriteLine($"\tReads: {item.TotalMessageStoreReads} (total), {item.MessageStoreReadDetails?.Value} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalMessageStoreWrites} (total), {item.MessageStoreWriteDetails?.Value} (rate)");
                Console.WriteLine();

                Console.WriteLine("Mnesia Transactions");
                Console.WriteLine($"\tDisk: {item.TotalMnesiaDiskTransactions} (total), {item.MnesiaDiskTransactionCountDetails?.Value} (rate)");
                Console.WriteLine($"\tRAM: {item.TotalMnesiaRamTransactions} (total), {item.MnesiaRAMTransactionCountDetails?.Value} (rate)");
                Console.WriteLine();
                
                Console.WriteLine("Queue Index");
                Console.WriteLine($"\tReads: {item.TotalQueueIndexReads} (total), {item.QueueIndexReadDetails?.Value} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalQueueIndexWrites} (total), {item.QueueIndexWriteDetails?.Value} (rate), {item.QueueIndexJournalWriteDetails?.Value} (journal)");
                Console.WriteLine();

                Console.WriteLine("IO");
                Console.WriteLine($"\tReads: {item.TotalIOReads} (total), {item.IOReadDetails?.Value} (rate)");
                Console.WriteLine($"\tWrites: {item.TotalIOWrites} (total), {item.IOWriteDetails?.Value} (rate)");
                Console.WriteLine("\tBytes");
                Console.WriteLine($"\t\tReads: {item.TotalIOBytesRead} (total), {item.IOBytesReadDetails?.Value} (rate)");
                Console.WriteLine($"\t\tWritten: {item.TotalIOBytesWritten} (total), {item.IOBytesWrittenDetails?.Value} (rate)");
                Console.WriteLine($"\tSeeks: {item.IOSeekCount} (total), {item.IOSeeksDetails?.Value} (rate), {item.AvgIOSeekTimeDetails?.Value} (avg. time)");
                Console.WriteLine($"\tSyncs: {item.IOSyncCount} (total), {item.IOSyncsDetails?.Value} (rate), {item.AvgIOSyncTimeDetails?.Value} (avg. time)");
                Console.WriteLine();
                
                Console.WriteLine("File Handles");
                Console.WriteLine($"\tOpen Attempts: {item.TotalOpenFileHandleAttempts} (total), {item.FileHandleOpenAttemptDetails?.Value} (rate), {item.FileHandleOpenAttemptAvgTimeDetails?.Value} (avg. time)");
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
                Console.WriteLine("\tGarbage Collection");
                Console.WriteLine($"\t\tMinor: {queue.GC?.MinorGarbageCollection}");
                Console.WriteLine($"\t\tHeap Size: {queue.GC?.MinimumHeapSize} (minimum), {queue.GC?.MaximumHeapSize} (maximum)");
                Console.WriteLine($"\t\tMinimum Binary Virtual Heap Size: {queue.GC?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine($"\t\tFull Sweep After: {queue.GC?.FullSweepAfter}");
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
                Console.WriteLine($"\tReductions: {queue.ReductionDetails?.Value} (rate)");
                Console.WriteLine($"\tMessages Unacknowledged: {queue.UnacknowledgedMessageDetails?.Value} (rate)");
                Console.WriteLine($"\tMessages Ready: {queue.ReadyMessageDetails?.Value} (rate)");
                Console.WriteLine($"\tMessages: {queue.MessageDetails?.Value} (rate)");
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
        
        public static Task<Result<SystemOverviewInfo>> ScreenDump(this Task<Result<SystemOverviewInfo>> result)
        {
            var item = result
                .GetResult()
                .Select(x => x.Data);
            
            Console.WriteLine($"Node: {item.Node}");
            Console.WriteLine($"Management Version: {item.ManagementVersion}");
            Console.WriteLine($"Cluster Name: {item.ClusterName}");
            Console.WriteLine($"Erlang Version: {item.ErlangVersion}");
            Console.WriteLine($"Erlang Version (Full): {item.ErlangFullVersion}");
            Console.WriteLine($"RMQ Version: {item.RabbitMqVersion}");
            Console.WriteLine($"Rates Mode: {item.RatesMode}");
            Console.WriteLine($"Total Disk Reads: {item.MessageStats?.TotalDiskReads}");
            Console.WriteLine($"Node: {item.StatsDatabaseEventQueue}");
            Console.WriteLine();
            
            Console.WriteLine($"Node: {item.Listeners}");
            Console.WriteLine($"Product Version: {item.ProductVersion}");
            Console.WriteLine($"Product Name: {item.ProductName}");

            foreach (long val in item.SampleRetentionPolicies.Basic)
                Console.WriteLine($"Basic: {val}");

            foreach (long val in item.SampleRetentionPolicies.Detailed)
                Console.WriteLine($"Detailed: {val}");

            foreach (long val in item.SampleRetentionPolicies.Global)
                Console.WriteLine($"Global: {val}");
            
            Console.WriteLine("Totals");
            Console.WriteLine($"\tChannels: {item.ObjectTotals?.TotalChannels}");
            Console.WriteLine($"\tConnections: {item.ObjectTotals?.TotalConnections}");
            Console.WriteLine($"\tConsumers: {item.ObjectTotals?.TotalConsumers}");
            Console.WriteLine($"\tExchanges: {item.ObjectTotals?.TotalExchanges}");
            Console.WriteLine($"\tQueues: {item.ObjectTotals?.TotalQueues}");
            Console.WriteLine();
            
            Console.WriteLine("Churn Rates");
            Console.WriteLine("\tChannels");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalChannelsCreated} (total), {item.ChurnRates?.CreatedChannelDetails?.Value} (rate)");
            Console.WriteLine($"\t\tClosed: {item.ChurnRates?.TotalChannelsClosed} (total), {item.ChurnRates?.ClosedChannelDetails?.Value} (rate)");
            Console.WriteLine("\tConnections");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalConnectionsCreated} (total), {item.ChurnRates?.CreatedConnectionDetails?.Value} (rate)");
            Console.WriteLine($"\t\tClosed: {item.ChurnRates?.TotalConnectionsClosed} (total), {item.ChurnRates?.ClosedConnectionDetails?.Value} (rate)");
            Console.WriteLine("\tQueues");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalQueuesCreated} (total), {item.ChurnRates?.CreatedQueueDetails?.Value} (rate)");
            Console.WriteLine($"\t\tDeclared: {item.ChurnRates?.TotalQueuesDeclared} (total), {item.ChurnRates?.DeclaredQueueDetails?.Value} (rate)");
            Console.WriteLine($"\t\tDeleted: {item.ChurnRates?.TotalQueuesDeleted} (total), {item.ChurnRates?.DeletedQueueDetails?.Value} (rate)");
            Console.WriteLine();
            
            Console.WriteLine("Queue Stats");
            Console.WriteLine($"\tMessages: {item.QueueStats?.TotalMessages} (total), {item.QueueStats?.MessageDetails} (rate)");
            Console.WriteLine($"\tUnacknowledged Delivered: {item.QueueStats?.TotalUnacknowledgedDeliveredMessages} (total), {item.QueueStats?.UnacknowledgedDeliveredMessagesDetails?.Value} (rate)");
            Console.WriteLine($"\tReady: {item.QueueStats?.TotalMessagesReadyForDelivery} (total), {item.QueueStats?.MessagesReadyForDeliveryDetails?.Value} (rate)");
            Console.WriteLine();

            Console.WriteLine("Listeners");
            foreach (var listener in item.Listeners)
            {
                Console.WriteLine($"\tNode: {listener.Node}");
                Console.WriteLine($"\tPort: {listener.Port}");
                Console.WriteLine($"\tProtocol: {listener.Protocol}");
                Console.WriteLine($"\tIP Address: {listener.IPAddress}");

                if (listener.SocketOptions.IsNull())
                    continue;

                Console.WriteLine("\tSocket Options");
                Console.WriteLine($"\t\tBacklog: {listener.SocketOptions.Backlog}");
                Console.WriteLine($"\t\tNo Delay: {listener.SocketOptions.NoDelay}");
                Console.WriteLine($"\t\tExit on Close: {listener.SocketOptions.ExitOnClose}");
                
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine("Contexts");
            foreach (var context in item.Contexts)
            {
                Console.WriteLine($"\tDescription: {context.Description}");
                Console.WriteLine($"\tPort: {context.Port}");
                Console.WriteLine($"\tPath: {context.Path}");
                Console.WriteLine("\t-------------------");
            }
            
            Console.WriteLine("****************************************************");
            Console.WriteLine();

            return result;
        }
    }
}