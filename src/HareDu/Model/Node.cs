namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface Node
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
        MemoryUsedDetails MemoryUsedDetails { get; }

        [JsonProperty("fd_used")]
        long FileDescriptorUsed { get; }

        [JsonProperty("fd_used_details")]
        FileDescriptorUsedDetails FileDescriptorUsedDetails { get; }

        [JsonProperty("sockets_used")]
        long SocketsUsed { get; }

        [JsonProperty("sockets_used_details")]
        SocketsUsedDetails SocketsUsedDetails { get; }

        [JsonProperty("proc_used")]
        long ProcessorUsed { get; }

        [JsonProperty("proc_used_details")]
        ProcessorUsedDetails ProcessorUsedDetails { get; }

        [JsonProperty("disk_free")]
        long DiskFree { get; }

        [JsonProperty("disk_free_details")]
        DiskFreeDetails DiskFreeDetails { get; }

        [JsonProperty("gc_num")]
        long GarbageCollectionNumber { get; }

        [JsonProperty("gc_num_details")]
        GarbageCollectionNumberDetails GarbageCollectionNumberDetails { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        long GarbageCollectionBytesReclaimed { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        GarbageCollectionBytesReclaimedDetails GarbageCollectionBytesReclaimedDetails { get; }

        [JsonProperty("context_switches")]
        long ContextSwitches { get; }

        [JsonProperty("context_switches_details")]
        ContextSwitchesDetails ContextSwitchesDetails { get; }

        [JsonProperty("io_read_count")]
        long IOReadCount { get; }

        [JsonProperty("io_read_count_details")]
        IOReadCountDetails IOReadCountDetails { get; }

        [JsonProperty("io_read_bytes")]
        long IOReadBytes { get; }

        [JsonProperty("io_read_bytes_details")]
        IOReadBytesDetails IOReadBytesDetails { get; }

        [JsonProperty("io_read_avg_time")]
        string IOReadAvgTime { get; }

        [JsonProperty("io_read_avg_time_details")]
        IOReadAvgTimeDetails IOReadAvgTimeDetails { get; }

        [JsonProperty("io_write_count")]
        long IOWriteCount { get; }

        [JsonProperty("io_write_count_details")]
        IOWriteCountDetails IOWriteCountDetails { get; }

        [JsonProperty("io_write_bytes")]
        long IOWriteBytes { get; }

        [JsonProperty("io_write_bytes_details")]
        IOWriteBytesDetails IOWriteBytesDetails { get; }

        [JsonProperty("io_write_avg_time")]
        decimal IOWriteAvgTime { get; }

        [JsonProperty("io_write_avg_time_details")]
        IOWriteAvgTimeDetails IOWriteAvgTimeDetails { get; }

        [JsonProperty("io_sync_count")]
        long IOSyncCount { get; }

        [JsonProperty("io_sync_count_details")]
        IOSyncCountDetails IOSyncCountDetails { get; }

        [JsonProperty("io_sync_avg_time")]
        decimal IOSyncAvgTime { get; }

        [JsonProperty("io_sync_avg_time_details")]
        IOSyncAvgTimeDetails IOSyncAvgTimeDetails { get; }

        [JsonProperty("io_seek_count")]
        long IOSeekCount { get; }

        [JsonProperty("io_seek_count_details")]
        IOSeekCountDetails IOSeekCountDetails { get; }

        [JsonProperty("io_seek_avg_time")]
        decimal IOSeekAvgTime { get; }

        [JsonProperty("io_seek_avg_time_details")]
        IOSeekAvgTimeDetails IOSeekAvgTimeDetails { get; }

        [JsonProperty("io_reopen_count")]
        long IOReopenCount { get; }

        [JsonProperty("io_reopen_count_details")]
        IOReopenCountDetails IOReopenCountDetails { get; }

        [JsonProperty("mnesia_ram_tx_count")]
        long MnesiaRamTransactionCount { get; }

        [JsonProperty("mnesia_ram_tx_count_details")]
        MnesiaRamTransactionCountDetails MnesiaRamTransactionCountDetails { get; }

        [JsonProperty("mnesia_disk_tx_count")]
        long MnesiaDiskTransactionCount { get; }

        [JsonProperty("mnesia_disk_tx_count_details")]
        MnesiaDiskTransactionCountDetails MnesiaDiskTransactionCountDetails { get; }

        [JsonProperty("msg_store_read_count")]
        long MessageStoreReadCount { get; }

        [JsonProperty("msg_store_read_count_details")]
        MessageStoreReadCountDetails MessageStoreReadCountDetails { get; }

        [JsonProperty("msg_store_write_count")]
        long MessageStoreWriteCount { get; }

        [JsonProperty("msg_store_write_count_details")]
        MessageStoreWriteCountDetails MessageStoreWriteCountDetails { get; }

        [JsonProperty("queue_index_journal_write_count")]
        long QueueIndexJournalWriteCount { get; }

        [JsonProperty("queue_index_journal_write_count_details")]
        QueueIndexJournalWriteCountDetails QueueIndexJournalWriteCountDetails { get; }

        [JsonProperty("queue_index_write_count")]
        long QueueIndexWriteCount { get; }

        [JsonProperty("queue_index_write_count_details")]
        QueueIndexWriteCountDetails QueueIndexWriteCountDetails { get; }

        [JsonProperty("queue_index_read_count")]
        long QueueIndexReadCount { get; }

        [JsonProperty("queue_index_read_count_details")]
        QueueIndexReadCountDetails QueueIndexReadCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_count")]
        long IOFileHandleOpenAttemptCount { get; }

        [JsonProperty("io_file_handle_open_attempt_count_details")]
        IOFileHandleOpenAttemptCountDetails IOFileHandleOpenAttemptCountDetails { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time")]
        decimal IOFileHandleOpenAttemptCountAverageTime { get; }

        [JsonProperty("io_file_handle_open_attempt_avg_time_details")]
        IOFileHandleOpenAttemptCountAvgTimeDetails IOFileHandleOpenAttemptCountAvgTimeDetails { get; }

        [JsonProperty("metrics_gc_queue_length")]
        MetricsGarbageCollectionQueueLength MetricsGarbageCollectionQueueLength { get; }
    }
}