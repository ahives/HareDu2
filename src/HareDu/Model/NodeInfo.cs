namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface NodeInfo
    {
        [JsonPropertyName("partitions")]
        IList<string> Partitions { get; }
        
        [JsonPropertyName("os_pid")]
        string OperatingSystemProcessId { get; }

        [JsonPropertyName("fd_total")]
        ulong TotalFileDescriptors { get; }

        [JsonPropertyName("sockets_total")]
        ulong TotalSocketsAvailable { get; }

        [JsonPropertyName("mem_limit")]
        ulong MemoryLimit { get; }

        [JsonPropertyName("mem_alarm")]
        bool MemoryAlarm { get; }

        [JsonPropertyName("disk_free_limit")]
        ulong FreeDiskLimit { get; }

        [JsonPropertyName("disk_free_alarm")]
        bool FreeDiskAlarm { get; }

        [JsonPropertyName("proc_total")]
        ulong TotalProcesses { get; }

        [JsonPropertyName("rates_mode")]
        string RatesMode { get; }

        [JsonPropertyName("uptime")]
        long Uptime { get; }

        [JsonPropertyName("run_queue")]
        int RunQueue { get; }

        [JsonPropertyName("type")]
        string Type { get; }
        
        [JsonPropertyName("processors")]
        uint AvailableCoresDetected { get; }

        [JsonPropertyName("exchange_types")]
        IList<ExchangeType> ExchangeTypes { get; }

        [JsonPropertyName("auth_mechanisms")]
        IList<AuthenticationMechanism> AuthenticationMechanisms { get; }

        [JsonPropertyName("applications")]
        IList<Application> Applications { get; }

        [JsonPropertyName("contexts")]
        IList<NodeContext> Contexts { get; }

        [JsonPropertyName("log_file")]
        string LogFile { get; }
        
        [JsonPropertyName("log_files")]
        IList<string> LogFiles { get; }

        [JsonPropertyName("sasl_log_file")]
        string SaslLogFile { get; }

        [JsonPropertyName("db_dir")]
        string DatabaseDirectory { get; }
        
        [JsonPropertyName("config_files")]
        IList<string> ConfigFiles { get; }

        [JsonPropertyName("net_ticktime")]
        long NetworkTickTime { get; }

        [JsonPropertyName("enabled_plugins")]
        IList<string> EnabledPlugins { get; }

        [JsonPropertyName("mem_calculation_strategy")]
        string MemoryCalculationStrategy { get; }

        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("running")]
        bool IsRunning { get; }

        [JsonPropertyName("mem_used")]
        ulong MemoryUsed { get; }

        [JsonPropertyName("mem_used_details")]
        Rate MemoryUsageDetails { get; }

        [JsonPropertyName("fd_used")]
        ulong FileDescriptorUsed { get; }

        [JsonPropertyName("fd_used_details")]
        Rate FileDescriptorUsedDetails { get; }

        [JsonPropertyName("sockets_used")]
        ulong SocketsUsed { get; }

        [JsonPropertyName("sockets_used_details")]
        Rate SocketsUsedDetails { get; }

        [JsonPropertyName("proc_used")]
        ulong ProcessesUsed { get; }

        [JsonPropertyName("proc_used_details")]
        Rate ProcessUsageDetails { get; }

        [JsonPropertyName("disk_free")]
        ulong FreeDiskSpace { get; }

        [JsonPropertyName("disk_free_details")]
        Rate FreeDiskSpaceDetails { get; }

        [JsonPropertyName("gc_num")]
        ulong NumberOfGarbageCollected { get; }

        [JsonPropertyName("gc_num_details")]
        Rate GcDetails { get; }

        [JsonPropertyName("gc_bytes_reclaimed")]
        ulong BytesReclaimedByGarbageCollector { get; }

        [JsonPropertyName("gc_bytes_reclaimed_details")]
        Rate ReclaimedBytesFromGCDetails { get; }

        [JsonPropertyName("context_switches")]
        ulong ContextSwitches { get; }

        [JsonPropertyName("context_switches_details")]
        Rate ContextSwitchDetails { get; }

        [JsonPropertyName("io_read_count")]
        ulong TotalIOReads { get; }

        [JsonPropertyName("io_read_count_details")]
        Rate IOReadDetails { get; }

        [JsonPropertyName("io_read_bytes")]
        ulong TotalIOBytesRead { get; }

        [JsonPropertyName("io_read_bytes_details")]
        Rate IOBytesReadDetails { get; }

        [JsonPropertyName("io_read_avg_time")]
        decimal AvgIOReadTime { get; }

        [JsonPropertyName("io_read_avg_time_details")]
        Rate AvgIOReadTimeDetails { get; }

        [JsonPropertyName("io_write_count")]
        ulong TotalIOWrites { get; }

        [JsonPropertyName("io_write_count_details")]
        Rate IOWriteDetails { get; }

        [JsonPropertyName("io_write_bytes")]
        ulong TotalIOBytesWritten { get; }

        [JsonPropertyName("io_write_bytes_details")]
        Rate IOBytesWrittenDetails { get; }

        [JsonPropertyName("io_write_avg_time")]
        decimal AvgTimePerIOWrite { get; }

        [JsonPropertyName("io_write_avg_time_details")]
        Rate AvgTimePerIOWriteDetails { get; }

        [JsonPropertyName("io_sync_count")]
        ulong IOSyncCount { get; }

        [JsonPropertyName("io_sync_count_details")]
        Rate IOSyncsDetails { get; }

        [JsonPropertyName("io_sync_avg_time")]
        decimal AverageIOSyncTime { get; }

        [JsonPropertyName("io_sync_avg_time_details")]
        Rate AvgIOSyncTimeDetails { get; }

        [JsonPropertyName("io_seek_count")]
        ulong IOSeekCount { get; }

        [JsonPropertyName("io_seek_count_details")]
        Rate IOSeeksDetails { get; }

        [JsonPropertyName("io_seek_avg_time")]
        decimal AverageIOSeekTime { get; }

        [JsonPropertyName("io_seek_avg_time_details")]
        Rate AvgIOSeekTimeDetails { get; }

        [JsonPropertyName("io_reopen_count")]
        ulong TotalIOReopened { get; }

        [JsonPropertyName("io_reopen_count_details")]
        Rate IOReopenedDetails { get; }

        [JsonPropertyName("mnesia_ram_tx_count")]
        ulong TotalMnesiaRamTransactions { get; }

        [JsonPropertyName("mnesia_ram_tx_count_details")]
        Rate MnesiaRAMTransactionCountDetails { get; }

        [JsonPropertyName("mnesia_disk_tx_count")]
        ulong TotalMnesiaDiskTransactions { get; }

        [JsonPropertyName("mnesia_disk_tx_count_details")]
        Rate MnesiaDiskTransactionCountDetails { get; }

        [JsonPropertyName("msg_store_read_count")]
        ulong TotalMessageStoreReads { get; }

        [JsonPropertyName("msg_store_read_count_details")]
        Rate MessageStoreReadDetails { get; }

        [JsonPropertyName("msg_store_write_count")]
        ulong TotalMessageStoreWrites { get; }

        [JsonPropertyName("msg_store_write_count_details")]
        Rate MessageStoreWriteDetails { get; }

        [JsonPropertyName("queue_index_journal_write_count")]
        ulong TotalQueueIndexJournalWrites { get; }

        [JsonPropertyName("queue_index_journal_write_count_details")]
        Rate QueueIndexJournalWriteDetails { get; }

        [JsonPropertyName("queue_index_write_count")]
        ulong TotalQueueIndexWrites { get; }

        [JsonPropertyName("queue_index_write_count_details")]
        Rate QueueIndexWriteDetails { get; }

        [JsonPropertyName("queue_index_read_count")]
        ulong TotalQueueIndexReads { get; }

        [JsonPropertyName("queue_index_read_count_details")]
        Rate QueueIndexReadDetails { get; }

        [JsonPropertyName("io_file_handle_open_attempt_count")]
        ulong TotalOpenFileHandleAttempts { get; }

        [JsonPropertyName("io_file_handle_open_attempt_count_details")]
        Rate FileHandleOpenAttemptDetails { get; }

        [JsonPropertyName("io_file_handle_open_attempt_avg_time")]
        decimal OpenFileHandleAttemptsAvgTime { get; }

        [JsonPropertyName("io_file_handle_open_attempt_avg_time_details")]
        Rate FileHandleOpenAttemptAvgTimeDetails { get; }

        [JsonPropertyName("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
        
        [JsonPropertyName("channel_closed")]
        ulong TotalChannelsClosed { get; }
        
        [JsonPropertyName("channel_closed_details")]
        Rate ClosedChannelDetails { get; }

        [JsonPropertyName("channel_created")]
        ulong TotalChannelsCreated { get; }
        
        [JsonPropertyName("channel_created_details")]
        Rate CreatedChannelDetails { get; }

        [JsonPropertyName("connection_closed")]
        ulong TotalConnectionsClosed { get; }
        
        [JsonPropertyName("connection_closed_details")]
        Rate ClosedConnectionDetails { get; }

        [JsonPropertyName("connection_created")]
        ulong TotalConnectionsCreated { get; }
        
        [JsonPropertyName("connection_created_details")]
        Rate CreatedConnectionDetails { get; }

        [JsonPropertyName("queue_created")]
        ulong TotalQueuesCreated { get; }
        
        [JsonPropertyName("queue_created_details")]
        Rate CreatedQueueDetails { get; }

        [JsonPropertyName("queue_declared")]
        ulong TotalQueuesDeclared { get; }
        
        [JsonPropertyName("queue_declared_details")]
        Rate DeclaredQueueDetails { get; }

        [JsonPropertyName("queue_deleted")]
        ulong TotalQueuesDeleted { get; }
        
        [JsonPropertyName("queue_deleted_details")]
        Rate DeletedQueueDetails { get; }
    }
}