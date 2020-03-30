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
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface NodeInfo
    {
        [JsonProperty("partitions")]
        IList<string> Partitions { get; }
        
        [JsonProperty("os_pid")]
        string OperatingSystemProcessId { get; }

        [JsonProperty("fd_total")]
        ulong TotalFileDescriptors { get; }

        [JsonProperty("sockets_total")]
        ulong TotalSocketsAvailable { get; }

        [JsonProperty("mem_limit")]
        ulong MemoryLimit { get; }

        [JsonProperty("mem_alarm")]
        bool MemoryAlarm { get; }

        [JsonProperty("disk_free_limit")]
        ulong FreeDiskLimit { get; }

        [JsonProperty("disk_free_alarm")]
        bool FreeDiskAlarm { get; }

        [JsonProperty("proc_total")]
        ulong TotalProcesses { get; }

        [JsonProperty("rates_mode")]
        string RatesMode { get; }

        [JsonProperty("uptime")]
        long Uptime { get; }

        [JsonProperty("run_queue")]
        int RunQueue { get; }

        [JsonProperty("type")]
        string Type { get; }
        
        [JsonProperty("processors")]
        uint AvailableCoresDetected { get; }

        [JsonProperty("exchange_types")]
        IList<ExchangeType> ExchangeTypes { get; }

        [JsonProperty("auth_mechanisms")]
        IList<AuthenticationMechanism> AuthenticationMechanisms { get; }

        [JsonProperty("applications")]
        IList<Application> Applications { get; }

        [JsonProperty("contexts")]
        IList<NodeContext> Contexts { get; }

        [JsonProperty("log_file")]
        string LogFile { get; }
        
        [JsonProperty("log_files")]
        IList<string> LogFiles { get; }

        [JsonProperty("sasl_log_file")]
        string SaslLogFile { get; }

        [JsonProperty("db_dir")]
        string DatabaseDirectory { get; }
        
        [JsonProperty("config_files")]
        IList<string> ConfigFiles { get; }

        [JsonProperty("net_ticktime")]
        long NetworkTickTime { get; }

        [JsonProperty("enabled_plugins")]
        IList<string> EnabledPlugins { get; }

        [JsonProperty("mem_calculation_strategy")]
        string MemoryCalculationStrategy { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("running")]
        bool IsRunning { get; }

        [JsonProperty("mem_used")]
        ulong MemoryUsed { get; }

        [JsonProperty("mem_used_details")]
        Rate MemoryUsageDetails { get; }

        [JsonProperty("fd_used")]
        ulong FileDescriptorUsed { get; }

        [JsonProperty("fd_used_details")]
        Rate FileDescriptorUsedDetails { get; }

        [JsonProperty("sockets_used")]
        ulong SocketsUsed { get; }

        [JsonProperty("sockets_used_details")]
        Rate SocketsUsedDetails { get; }

        [JsonProperty("proc_used")]
        ulong ProcessesUsed { get; }

        [JsonProperty("proc_used_details")]
        ProcessUsageDetails ProcessUsageDetails { get; }

        [JsonProperty("disk_free")]
        ulong FreeDiskSpace { get; }

        [JsonProperty("disk_free_details")]
        Rate FreeDiskSpaceDetails { get; }

        [JsonProperty("gc_num")]
        ulong NumberOfGarbageCollected { get; }

        [JsonProperty("gc_num_details")]
        Rate GcDetails { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        ulong BytesReclaimedByGarbageCollector { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        Rate ReclaimedBytesFromGCDetails { get; }

        [JsonProperty("context_switches")]
        ulong ContextSwitches { get; }

        [JsonProperty("context_switches_details")]
        Rate ContextSwitchDetails { get; }

        [JsonProperty("io_read_count")]
        ulong TotalIOReads { get; }

        [JsonProperty("io_read_count_details")]
        Rate IOReadDetails { get; }

        [JsonProperty("io_read_bytes")]
        ulong TotalIOBytesRead { get; }

        [JsonProperty("io_read_bytes_details")]
        Rate IOReadBytesDetails { get; }

        [JsonProperty("io_read_avg_time")]
        decimal AvgIOReadTime { get; }

        [JsonProperty("io_read_avg_time_details")]
        Rate AvgIOReadTimeDetails { get; }

        [JsonProperty("io_write_count")]
        ulong TotalIOWrites { get; }

        [JsonProperty("io_write_count_details")]
        Rate IOWriteDetails { get; }

        [JsonProperty("io_write_bytes")]
        ulong TotalIOBytesWritten { get; }

        [JsonProperty("io_write_bytes_details")]
        Rate IOWriteBytesDetails { get; }

        [JsonProperty("io_write_avg_time")]
        decimal AvgTimePerIOWrite { get; }

        [JsonProperty("io_write_avg_time_details")]
        Rate AvgTimePerIOWriteDetails { get; }

        [JsonProperty("io_sync_count")]
        ulong IOSyncCount { get; }

        [JsonProperty("io_sync_count_details")]
        Rate IOSyncsDetails { get; }

        [JsonProperty("io_sync_avg_time")]
        decimal AverageIOSyncTime { get; }

        [JsonProperty("io_sync_avg_time_details")]
        Rate AvgIOSyncTimeDetails { get; }

        [JsonProperty("io_seek_count")]
        ulong IOSeekCount { get; }

        [JsonProperty("io_seek_count_details")]
        Rate IOSeeksDetails { get; }

        [JsonProperty("io_seek_avg_time")]
        decimal AverageIOSeekTime { get; }

        [JsonProperty("io_seek_avg_time_details")]
        Rate AvgIOSeekTimeDetails { get; }

        [JsonProperty("io_reopen_count")]
        ulong IOReopenCount { get; }

        [JsonProperty("io_reopen_count_details")]
        Rate RateOfIOReopened { get; }

        [JsonProperty("mnesia_ram_tx_count")]
        ulong TotalMnesiaRamTransactions { get; }

        [JsonProperty("mnesia_ram_tx_count_details")]
        Rate MnesiaRAMTransactionCountDetails { get; }

        [JsonProperty("mnesia_disk_tx_count")]
        ulong TotalMnesiaDiskTransactions { get; }

        [JsonProperty("mnesia_disk_tx_count_details")]
        Rate MnesiaDiskTransactionCountDetails { get; }

        [JsonProperty("msg_store_read_count")]
        ulong TotalMessageStoreReads { get; }

        [JsonProperty("msg_store_read_count_details")]
        Rate MessageStoreReadCountDetails { get; }

        [JsonProperty("msg_store_write_count")]
        ulong TotalMessageStoreWrites { get; }

        [JsonProperty("msg_store_write_count_details")]
        Rate MessageStoreWriteCountDetails { get; }

        [JsonProperty("queue_index_journal_write_count")]
        ulong TotalQueueIndexJournalWrites { get; }

        [JsonProperty("queue_index_journal_write_count_details")]
        Rate QueueIndexJournalWriteCountDetails { get; }

        [JsonProperty("queue_index_write_count")]
        ulong TotalQueueIndexWrites { get; }

        [JsonProperty("queue_index_write_count_details")]
        Rate QueueIndexWriteCountDetails { get; }

        [JsonProperty("queue_index_read_count")]
        ulong TotalQueueIndexReads { get; }

        [JsonProperty("queue_index_read_count_details")]
        Rate QueueIndexReadCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_count")]
        ulong TotalOpenFileHandleAttempts { get; }

        [JsonProperty("io_file_handle_open_attempt_count_details")]
        Rate FileHandleOpenAttemptCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time")]
        decimal OpenFileHandleAttemptsAvgTime { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time_details")]
        Rate FileHandleOpenAttemptAvgTimeDetails { get; }

        [JsonProperty("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
        
        [JsonProperty("channel_closed")]
        ulong TotalChannelsClosed { get; }
        
        [JsonProperty("channel_closed_details")]
        Rate ClosedChannelDetails { get; }

        [JsonProperty("channel_created")]
        ulong TotalChannelsCreated { get; }
        
        [JsonProperty("channel_created_details")]
        Rate CreatedChannelDetails { get; }

        [JsonProperty("connection_closed")]
        ulong TotalConnectionsClosed { get; }
        
        [JsonProperty("connection_closed_details")]
        Rate ClosedConnectionDetails { get; }

        [JsonProperty("connection_created")]
        ulong TotalConnectionsCreated { get; }
        
        [JsonProperty("connection_created_details")]
        Rate CreatedConnectionDetails { get; }

        [JsonProperty("queue_created")]
        ulong TotalQueuesCreated { get; }
        
        [JsonProperty("queue_created_details")]
        Rate CreatedQueueDetails { get; }

        [JsonProperty("queue_declared")]
        ulong TotalQueuesDeclared { get; }
        
        [JsonProperty("queue_declared_details")]
        Rate DeclaredQueueDetails { get; }

        [JsonProperty("queue_deleted")]
        ulong TotalQueuesDeleted { get; }
        
        [JsonProperty("queue_deleted_details")]
        Rate DeletedQueueDetails { get; }
    }
}