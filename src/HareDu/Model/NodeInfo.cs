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

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("running")]
        bool IsRunning { get; }

        [JsonProperty("mem_used")]
        ulong MemoryUsed { get; }

        [JsonProperty("mem_used_details")]
        MemoryUsageDetails MemoryUsageDetails { get; }

        [JsonProperty("fd_used")]
        ulong FileDescriptorUsed { get; }

        [JsonProperty("fd_used_details")]
        FileDescriptorUsedDetails FileDescriptorUsedDetails { get; }

        [JsonProperty("sockets_used")]
        ulong SocketsUsed { get; }

        [JsonProperty("sockets_used_details")]
        SocketsUsedDetails SocketsUsedDetails { get; }

        [JsonProperty("proc_used")]
        ulong ProcessesUsed { get; }

        [JsonProperty("proc_used_details")]
        ProcessUsageDetails ProcessUsageDetails { get; }

        [JsonProperty("disk_free")]
        ulong FreeDiskSpace { get; }

        [JsonProperty("disk_free_details")]
        FreeDiskSpaceDetails FreeDiskSpaceDetails { get; }

        [JsonProperty("gc_num")]
        ulong NumberOfGarbageCollected { get; }

        [JsonProperty("gc_num_details")]
        GCDetails GcDetails { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        ulong ReclaimedBytesFromGC { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        ReclaimedBytesFromGCDetails ReclaimedBytesFromGCDetails { get; }

        [JsonProperty("context_switches")]
        long ContextSwitches { get; }

        [JsonProperty("context_switches_details")]
        ContextSwitchDetails ContextSwitchDetails { get; }

        [JsonProperty("io_read_count")]
        ulong TotalIOReads { get; }

        [JsonProperty("io_read_count_details")]
        IOReadCountDetails IOReadCountDetails { get; }

        [JsonProperty("io_read_bytes")]
        ulong TotalIOBytesRead { get; }

        [JsonProperty("io_read_bytes_details")]
        IOBytesReadDetails IOReadBytesDetails { get; }

        [JsonProperty("io_read_avg_time")]
        ulong AvgIOReadTime { get; }

        [JsonProperty("io_read_avg_time_details")]
        AvgIOReadTimeDetails AvgIOReadTimeDetails { get; }

        [JsonProperty("io_write_count")]
        ulong TotalIOWrites { get; }

        [JsonProperty("io_write_count_details")]
        IOWriteDetails IOWriteDetails { get; }

        [JsonProperty("io_write_bytes")]
        ulong TotalIOWriteBytes { get; }

        [JsonProperty("io_write_bytes_details")]
        IOWriteBytesDetail IOWriteBytesDetail { get; }

        [JsonProperty("io_write_avg_time")]
        ulong AvgTimePerIOWrite { get; }

        [JsonProperty("io_write_avg_time_details")]
        AvgTimePerIOWriteDetails AvgTimePerIOWriteDetails { get; }

        [JsonProperty("io_sync_count")]
        ulong IOSyncCount { get; }

        [JsonProperty("io_sync_count_details")]
        IOSyncCountDetails RateOfIOSyncs { get; }

        [JsonProperty("io_sync_avg_time")]
        decimal AverageIOSyncTime { get; }

        [JsonProperty("io_sync_avg_time_details")]
        AvgIOSyncTimeDetails AvgIOSyncTimeDetails { get; }

        [JsonProperty("io_seek_count")]
        ulong IOSeekCount { get; }

        [JsonProperty("io_seek_count_details")]
        IOSeekCountDetails RateOfIOSeeks { get; }

        [JsonProperty("io_seek_avg_time")]
        ulong AverageIOSeekTime { get; }

        [JsonProperty("io_seek_avg_time_details")]
        AvgIOSeekTimeDetails AvgIOSeekTimeDetails { get; }

        [JsonProperty("io_reopen_count")]
        ulong IOReopenCount { get; }

        [JsonProperty("io_reopen_count_details")]
        IOReopenCountDetails RateOfIOReopened { get; }

        [JsonProperty("mnesia_ram_tx_count")]
        ulong TotalMnesiaRamTransactions { get; }

        [JsonProperty("mnesia_ram_tx_count_details")]
        MnesiaRAMTransactionCountDetails MnesiaRAMTransactionCountDetails { get; }

        [JsonProperty("mnesia_disk_tx_count")]
        ulong TotalMnesiaDiskTransactions { get; }

        [JsonProperty("mnesia_disk_tx_count_details")]
        MnesiaDiskTransactionCountDetails MnesiaDiskTransactionCountDetails { get; }

        [JsonProperty("msg_store_read_count")]
        ulong TotalMessageStoreReads { get; }

        [JsonProperty("msg_store_read_count_details")]
        MessageStoreReadCountDetails MessageStoreReadCountDetails { get; }

        [JsonProperty("msg_store_write_count")]
        ulong TotalMessageStoreWrites { get; }

        [JsonProperty("msg_store_write_count_details")]
        MessageStoreWriteCountDetails MessageStoreWriteCountDetails { get; }

        [JsonProperty("queue_index_journal_write_count")]
        ulong TotalQueueIndexJournalWrites { get; }

        [JsonProperty("queue_index_journal_write_count_details")]
        QueueIndexJournalWriteCountDetails QueueIndexJournalWriteCountDetails { get; }

        [JsonProperty("queue_index_write_count")]
        ulong TotalQueueIndexWrites { get; }

        [JsonProperty("queue_index_write_count_details")]
        QueueIndexWriteCountDetails QueueIndexWriteCountDetails { get; }

        [JsonProperty("queue_index_read_count")]
        ulong TotalQueueIndexReads { get; }

        [JsonProperty("queue_index_read_count_details")]
        QueueIndexReadCountDetails QueueIndexReadCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_count")]
        ulong TotalOpenFileHandleAttempts { get; }

        [JsonProperty("io_file_handle_open_attempt_count_details")]
        FileHandleOpenAttemptCountDetails FileHandleOpenAttemptCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time")]
        decimal OpenFileHandleAttemptsAvgTime { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time_details")]
        FileHandleOpenAttemptAvgTimeDetails FileHandleOpenAttemptAvgTimeDetails { get; }

        [JsonProperty("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
    }
}