// Copyright 2013-2017 Albert L. Hives
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
        [JsonProperty("os_pid")]
        string OperatingSystemPid { get; }

        [JsonProperty("fd_total")]
        long TotalFileDescriptors { get; }

        [JsonProperty("sockets_total")]
        long TotalSockets { get; }

        [JsonProperty("mem_limit")]
        long MemoryLimit { get; }

        [JsonProperty("mem_alarm")]
        bool MemoryAlarm { get; }

        [JsonProperty("disk_free_limit")]
        string DiskFreeLimit { get; }

        [JsonProperty("disk_free_alarm")]
        bool DiskFreeAlarm { get; }

        [JsonProperty("proc_total")]
        long TotalProcessors { get; }

        [JsonProperty("rates_mode")]
        string RatesMode { get; }

        [JsonProperty("uptime")]
        long Uptime { get; }

        [JsonProperty("run_queue")]
        int RunQueue { get; }

        [JsonProperty("processors")]
        int Processors { get; }

        [JsonProperty("exchange_types")]
        IEnumerable<ExchangeType> ExchangeTypes { get; }

        [JsonProperty("auth_mechanisms")]
        IEnumerable<AuthenticationMechanism> AuthenticationMechanisms { get; }

        [JsonProperty("applications")]
        IEnumerable<Application> Applications { get; }

        [JsonProperty("contexts")]
        IEnumerable<Context> Contexts { get; }

        [JsonProperty("log_file")]
        string LogFile { get; }

        [JsonProperty("sasl_log_file")]
        string SaslLogFile { get; }

        [JsonProperty("db_dir")]
        string DbDirectory { get; }

        [JsonProperty("net_ticktime")]
        long NetworkTickTime { get; }

        [JsonProperty("enabled_plugins")]
        IEnumerable<string> EnabledPlugins { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("running")]
        bool IsRunning { get; }

        [JsonProperty("mem_used")]
        long MemoryUsed { get; }

        [JsonProperty("mem_used_details")]
        Rate MemoryUsedRate { get; }

        [JsonProperty("fd_used")]
        long FileDescriptorUsed { get; }

        [JsonProperty("fd_used_details")]
        Rate FileDescriptorUsedRate { get; }

        [JsonProperty("sockets_used")]
        long SocketsUsed { get; }

        [JsonProperty("sockets_used_details")]
        Rate SocketsUsedRate { get; }

        [JsonProperty("proc_used")]
        long ProcessorsUsed { get; }

        [JsonProperty("proc_used_details")]
        Rate ProcessorsUsedRate { get; }

        [JsonProperty("disk_free")]
        long DiskFree { get; }

        [JsonProperty("disk_free_details")]
        Rate RateOfFreeDiskSpace { get; }

        [JsonProperty("gc_num")]
        long NumberOfGarbageCollected { get; }

        [JsonProperty("gc_num_details")]
        Rate RateOfGarbageCollected { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        long ReclaimedBytesGarbageCollected { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        Rate RateOfReclaimedBytesGarbageCollected { get; }

        [JsonProperty("context_switches")]
        long ContextSwitches { get; }

        [JsonProperty("context_switches_details")]
        Rate RateOfContextSwitches { get; }

        [JsonProperty("io_read_count")]
        long IOReads { get; }

        [JsonProperty("io_read_count_details")]
        Rate RateOfIOReads { get; }

        [JsonProperty("io_read_bytes")]
        long TotalIOBytesRead { get; }

        [JsonProperty("io_read_bytes_details")]
        Rate RateOfIOBytesRead { get; }

        [JsonProperty("io_read_avg_time")]
        string AvgIOReadTime { get; }

        [JsonProperty("io_read_avg_time_details")]
        Rate RateOfAvgIOReadTime { get; }

        [JsonProperty("io_write_count")]
        long TotalIOWrites { get; }

        [JsonProperty("io_write_count_details")]
        Rate RateOfIOWrites { get; }

        [JsonProperty("io_write_bytes")]
        long IOWriteBytes { get; }

        [JsonProperty("io_write_bytes_details")]
        Rate RateOfIOWritesInBytes { get; }

        [JsonProperty("io_write_avg_time")]
        decimal IOWriteAvgTime { get; }

        [JsonProperty("io_write_avg_time_details")]
        Rate RateOfAvgIOWriteTime { get; }

        [JsonProperty("io_sync_count")]
        long TotalIOSyncs { get; }

        [JsonProperty("io_sync_count_details")]
        Rate RateOfIOSyncs { get; }

        [JsonProperty("io_sync_avg_time")]
        decimal AvgIOSyncTime { get; }

        [JsonProperty("io_sync_avg_time_details")]
        Rate RateOfAvgIOSyncTime { get; }

        [JsonProperty("io_seek_count")]
        long TotalIOSeeks { get; }

        [JsonProperty("io_seek_count_details")]
        Rate RateOfIOSeeks { get; }

        [JsonProperty("io_seek_avg_time")]
        decimal AvgIOSeekTime { get; }

        [JsonProperty("io_seek_avg_time_details")]
        Rate RateOfAvgIOSeekTime { get; }

        [JsonProperty("io_reopen_count")]
        long TotalIOReopened { get; }

        [JsonProperty("io_reopen_count_details")]
        Rate RateOfIOReopened { get; }

        [JsonProperty("mnesia_ram_tx_count")]
        long TotalMnesiaRamTransactions { get; }

        [JsonProperty("mnesia_ram_tx_count_details")]
        Rate RateOfMnesiaRamTransactions { get; }

        [JsonProperty("mnesia_disk_tx_count")]
        long TotalMnesiaDiskTransactions { get; }

        [JsonProperty("mnesia_disk_tx_count_details")]
        Rate RateOfMnesiaDiskTransactions { get; }

        [JsonProperty("msg_store_read_count")]
        long TotalMessageStoreReads { get; }

        [JsonProperty("msg_store_read_count_details")]
        Rate RateOfMessageStoreReads { get; }

        [JsonProperty("msg_store_write_count")]
        long TotalMessageStoreWrites { get; }

        [JsonProperty("msg_store_write_count_details")]
        Rate RateOfMessageStoreWrites { get; }

        [JsonProperty("queue_index_journal_write_count")]
        long TotalQueueIndexJournalWrites { get; }

        [JsonProperty("queue_index_journal_write_count_details")]
        Rate RateOfQueueIndexJournalWrites { get; }

        [JsonProperty("queue_index_write_count")]
        long TotalQueueIndexWrites { get; }

        [JsonProperty("queue_index_write_count_details")]
        Rate RateOfQueueIndexWrites { get; }

        [JsonProperty("queue_index_read_count")]
        long TotalQueueIndexReads { get; }

        [JsonProperty("queue_index_read_count_details")]
        Rate RateOfQueueIndexReads { get; }

        [JsonProperty("io_file_handle_open_attempt_count")]
        long TotalOpenFileHandleAttempts { get; }

        [JsonProperty("io_file_handle_open_attempt_count_details")]
        Rate RateOfOpenFileHandleAttempts { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time")]
        decimal OpenFileHandleAttemptsAvgTime { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time_details")]
        Rate RateOfOpenFileHandleAttemptsAvgTime { get; }

        [JsonProperty("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
    }
}