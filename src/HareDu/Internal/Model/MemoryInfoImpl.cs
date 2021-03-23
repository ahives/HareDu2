namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class MemoryInfoImpl
    {
        [JsonPropertyName("connection_readers")]
        public long ConnectionReaders { get; set; }
        
        [JsonPropertyName("connection_writers")]
        public long ConnectionWriters { get; set; }
        
        [JsonPropertyName("connection_channels")]
        public long ConnectionChannels { get; set; }
        
        [JsonPropertyName("connection_other")]
        public long ConnectionOther { get; set; }
        
        [JsonPropertyName("queue_procs")]
        public long QueueProcesses { get; set; }
        
        [JsonPropertyName("queue_slave_procs")]
        public long QueueSlaveProcesses { get; set; }
        
        [JsonPropertyName("plugins")]
        public long Plugins { get; set; }
        
        [JsonPropertyName("other_proc")]
        public long OtherProcesses { get; set; }
        
        [JsonPropertyName("metrics")]
        public long Metrics { get; set; }
        
        [JsonPropertyName("mgmt_db")]
        public long ManagementDatabase { get; set; }
        
        [JsonPropertyName("mnesia")]
        public long Mnesia { get; set; }
        
        [JsonPropertyName("other_ets")]
        public long OtherInMemoryStorage { get; set; }
        
        [JsonPropertyName("binary")]
        public long Binary { get; set; }
        
        [JsonPropertyName("msg_index")]
        public long MessageIndex { get; set; }
        
        [JsonPropertyName("code")]
        public long ByteCode { get; set; }
        
        [JsonPropertyName("atom")]
        public long Atom { get; set; }
        
        [JsonPropertyName("other_system")]
        public long OtherSystem { get; set; }
        
        [JsonPropertyName("allocated_unused")]
        public long AllocatedUnused { get; set; }
        
        [JsonPropertyName("reserved_unallocated")]
        public long ReservedUnallocated { get; set; }
        
        [JsonPropertyName("strategy")]
        public string Strategy { get; set; }
        
        [JsonPropertyName("total")]
        public TotalMemoryInfoImpl Total { get; set; }
        
        [JsonPropertyName("quorum_queue_procs")]
        public long QuorumQueueProcesses { get; set; }
        
        [JsonPropertyName("quorum_ets")]
        public long QuorumInMemoryStorage { get; set; }
    }
}