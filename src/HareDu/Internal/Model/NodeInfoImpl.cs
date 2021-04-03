namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class NodeInfoImpl
    {
        [JsonPropertyName("partitions")]
        public IList<string> Partitions { get; set; }
        
        [JsonPropertyName("os_pid")]
        public string OperatingSystemProcessId { get; set; }

        [JsonPropertyName("fd_total")]
        public ulong TotalFileDescriptors { get; set; }

        [JsonPropertyName("sockets_total")]
        public ulong TotalSocketsAvailable { get; set; }

        [JsonPropertyName("mem_limit")]
        public ulong MemoryLimit { get; set; }

        [JsonPropertyName("mem_alarm")]
        public bool MemoryAlarm { get; set; }

        [JsonPropertyName("disk_free_limit")]
        public ulong FreeDiskLimit { get; set; }

        [JsonPropertyName("disk_free_alarm")]
        public bool FreeDiskAlarm { get; set; }

        [JsonPropertyName("proc_total")]
        public ulong TotalProcesses { get; set; }

        [JsonPropertyName("rates_mode")]
        public RatesMode RatesMode { get; set; }

        [JsonPropertyName("uptime")]
        public long Uptime { get; set; }

        [JsonPropertyName("run_queue")]
        public int RunQueue { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("processors")]
        public uint AvailableCoresDetected { get; set; }

        [JsonPropertyName("exchange_types")]
        public IList<ExchangeTypeImpl> ExchangeTypes { get; set; }

        [JsonPropertyName("auth_mechanisms")]
        public IList<AuthenticationMechanismImpl> AuthenticationMechanisms { get; set; }

        [JsonPropertyName("applications")]
        public IList<ApplicationImpl> Applications { get; set; }

        [JsonPropertyName("contexts")]
        public IList<NodeContextImpl> Contexts { get; set; }

        [JsonPropertyName("log_file")]
        public string LogFile { get; set; }
        
        [JsonPropertyName("log_files")]
        public IList<string> LogFiles { get; set; }

        [JsonPropertyName("sasl_log_file")]
        public string SaslLogFile { get; set; }

        [JsonPropertyName("db_dir")]
        public string DatabaseDirectory { get; set; }
        
        [JsonPropertyName("config_files")]
        public IList<string> ConfigFiles { get; set; }

        [JsonPropertyName("net_ticktime")]
        public long NetworkTickTime { get; set; }

        [JsonPropertyName("enabled_plugins")]
        public IList<string> EnabledPlugins { get; set; }

        [JsonPropertyName("mem_calculation_strategy")]
        public string MemoryCalculationStrategy { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("running")]
        public bool IsRunning { get; set; }

        [JsonPropertyName("mem_used")]
        public ulong MemoryUsed { get; set; }

        [JsonPropertyName("mem_used_details")]
        public RateImpl MemoryUsageDetails { get; set; }

        [JsonPropertyName("fd_used")]
        public ulong FileDescriptorUsed { get; set; }

        [JsonPropertyName("fd_used_details")]
        public RateImpl FileDescriptorUsedDetails { get; set; }

        [JsonPropertyName("sockets_used")]
        public ulong SocketsUsed { get; set; }

        [JsonPropertyName("sockets_used_details")]
        public RateImpl SocketsUsedDetails { get; set; }

        [JsonPropertyName("proc_used")]
        public ulong ProcessesUsed { get; set; }

        [JsonPropertyName("proc_used_details")]
        public RateImpl ProcessUsageDetails { get; set; }

        [JsonPropertyName("disk_free")]
        public ulong FreeDiskSpace { get; set; }

        [JsonPropertyName("disk_free_details")]
        public RateImpl FreeDiskSpaceDetails { get; set; }

        [JsonPropertyName("gc_num")]
        public ulong NumberOfGarbageCollected { get; set; }

        [JsonPropertyName("gc_num_details")]
        public RateImpl GcDetails { get; set; }

        [JsonPropertyName("gc_bytes_reclaimed")]
        public ulong BytesReclaimedByGarbageCollector { get; set; }

        [JsonPropertyName("gc_bytes_reclaimed_details")]
        public RateImpl ReclaimedBytesFromGCDetails { get; set; }

        [JsonPropertyName("context_switches")]
        public ulong ContextSwitches { get; set; }

        [JsonPropertyName("context_switches_details")]
        public RateImpl ContextSwitchDetails { get; set; }

        [JsonPropertyName("io_read_count")]
        public ulong TotalIOReads { get; set; }

        [JsonPropertyName("io_read_count_details")]
        public RateImpl IOReadDetails { get; set; }

        [JsonPropertyName("io_read_bytes")]
        public ulong TotalIOBytesRead { get; set; }

        [JsonPropertyName("io_read_bytes_details")]
        public RateImpl IOBytesReadDetails { get; set; }

        [JsonPropertyName("io_read_avg_time")]
        public decimal AvgIOReadTime { get; set; }

        [JsonPropertyName("io_read_avg_time_details")]
        public RateImpl AvgIOReadTimeDetails { get; set; }

        [JsonPropertyName("io_write_count")]
        public ulong TotalIOWrites { get; set; }

        [JsonPropertyName("io_write_count_details")]
        public RateImpl IOWriteDetails { get; set; }

        [JsonPropertyName("io_write_bytes")]
        public ulong TotalIOBytesWritten { get; set; }

        [JsonPropertyName("io_write_bytes_details")]
        public RateImpl IOBytesWrittenDetails { get; set; }

        [JsonPropertyName("io_write_avg_time")]
        public decimal AvgTimePerIOWrite { get; set; }

        [JsonPropertyName("io_write_avg_time_details")]
        public RateImpl AvgTimePerIOWriteDetails { get; set; }

        [JsonPropertyName("io_sync_count")]
        public ulong IOSyncCount { get; set; }

        [JsonPropertyName("io_sync_count_details")]
        public RateImpl IOSyncsDetails { get; set; }

        [JsonPropertyName("io_sync_avg_time")]
        public decimal AverageIOSyncTime { get; set; }

        [JsonPropertyName("io_sync_avg_time_details")]
        public RateImpl AvgIOSyncTimeDetails { get; set; }

        [JsonPropertyName("io_seek_count")]
        public ulong IOSeekCount { get; set; }

        [JsonPropertyName("io_seek_count_details")]
        public RateImpl IOSeeksDetails { get; set; }

        [JsonPropertyName("io_seek_avg_time")]
        public decimal AverageIOSeekTime { get; set; }

        [JsonPropertyName("io_seek_avg_time_details")]
        public RateImpl AvgIOSeekTimeDetails { get; set; }

        [JsonPropertyName("io_reopen_count")]
        public ulong TotalIOReopened { get; set; }

        [JsonPropertyName("io_reopen_count_details")]
        public RateImpl IOReopenedDetails { get; set; }

        [JsonPropertyName("mnesia_ram_tx_count")]
        public ulong TotalMnesiaRamTransactions { get; set; }

        [JsonPropertyName("mnesia_ram_tx_count_details")]
        public RateImpl MnesiaRAMTransactionCountDetails { get; set; }

        [JsonPropertyName("mnesia_disk_tx_count")]
        public ulong TotalMnesiaDiskTransactions { get; set; }

        [JsonPropertyName("mnesia_disk_tx_count_details")]
        public RateImpl MnesiaDiskTransactionCountDetails { get; set; }

        [JsonPropertyName("msg_store_read_count")]
        public ulong TotalMessageStoreReads { get; set; }

        [JsonPropertyName("msg_store_read_count_details")]
        public RateImpl MessageStoreReadDetails { get; set; }

        [JsonPropertyName("msg_store_write_count")]
        public ulong TotalMessageStoreWrites { get; set; }

        [JsonPropertyName("msg_store_write_count_details")]
        public RateImpl MessageStoreWriteDetails { get; set; }

        [JsonPropertyName("queue_index_journal_write_count")]
        public ulong TotalQueueIndexJournalWrites { get; set; }

        [JsonPropertyName("queue_index_journal_write_count_details")]
        public RateImpl QueueIndexJournalWriteDetails { get; set; }

        [JsonPropertyName("queue_index_write_count")]
        public ulong TotalQueueIndexWrites { get; set; }

        [JsonPropertyName("queue_index_write_count_details")]
        public RateImpl QueueIndexWriteDetails { get; set; }

        [JsonPropertyName("queue_index_read_count")]
        public ulong TotalQueueIndexReads { get; set; }

        [JsonPropertyName("queue_index_read_count_details")]
        public RateImpl QueueIndexReadDetails { get; set; }

        [JsonPropertyName("io_file_handle_open_attempt_count")]
        public ulong TotalOpenFileHandleAttempts { get; set; }

        [JsonPropertyName("io_file_handle_open_attempt_count_details")]
        public RateImpl FileHandleOpenAttemptDetails { get; set; }

        [JsonPropertyName("io_file_handle_open_attempt_avg_time")]
        public decimal OpenFileHandleAttemptsAvgTime { get; set; }

        [JsonPropertyName("io_file_handle_open_attempt_avg_time_details")]
        public RateImpl FileHandleOpenAttemptAvgTimeDetails { get; set; }

        [JsonPropertyName("metrics_gc_queue_length")]
        public GarbageCollectionMetricsImpl GarbageCollectionMetrics { get; set; }
        
        [JsonPropertyName("channel_closed")]
        public ulong TotalChannelsClosed { get; set; }
        
        [JsonPropertyName("channel_closed_details")]
        public RateImpl ClosedChannelDetails { get; set; }

        [JsonPropertyName("channel_created")]
        public ulong TotalChannelsCreated { get; set; }
        
        [JsonPropertyName("channel_created_details")]
        public RateImpl CreatedChannelDetails { get; set; }

        [JsonPropertyName("connection_closed")]
        public ulong TotalConnectionsClosed { get; set; }
        
        [JsonPropertyName("connection_closed_details")]
        public RateImpl ClosedConnectionDetails { get; set; }

        [JsonPropertyName("connection_created")]
        public ulong TotalConnectionsCreated { get; set; }
        
        [JsonPropertyName("connection_created_details")]
        public RateImpl CreatedConnectionDetails { get; set; }

        [JsonPropertyName("queue_created")]
        public ulong TotalQueuesCreated { get; set; }
        
        [JsonPropertyName("queue_created_details")]
        public RateImpl CreatedQueueDetails { get; set; }

        [JsonPropertyName("queue_declared")]
        public ulong TotalQueuesDeclared { get; set; }
        
        [JsonPropertyName("queue_declared_details")]
        public RateImpl DeclaredQueueDetails { get; set; }

        [JsonPropertyName("queue_deleted")]
        public ulong TotalQueuesDeleted { get; set; }
        
        [JsonPropertyName("queue_deleted_details")]
        public RateImpl DeletedQueueDetails { get; set; }
    }
}