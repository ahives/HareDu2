namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface MemoryInfo
    {
        [JsonPropertyName("connection_readers")]
        long ConnectionReaders { get; }
        
        [JsonPropertyName("connection_writers")]
        long ConnectionWriters { get; }
        
        [JsonPropertyName("connection_channels")]
        long ConnectionChannels { get; }
        
        [JsonPropertyName("connection_other")]
        long ConnectionOther { get; }
        
        [JsonPropertyName("queue_procs")]
        long QueueProcesses { get; }
        
        [JsonPropertyName("queue_slave_procs")]
        long QueueSlaveProcesses { get; }
        
        [JsonPropertyName("plugins")]
        long Plugins { get; }
        
        [JsonPropertyName("other_proc")]
        long OtherProcesses { get; }
        
        [JsonPropertyName("metrics")]
        long Metrics { get; }
        
        [JsonPropertyName("mgmt_db")]
        long ManagementDatabase { get; }
        
        [JsonPropertyName("mnesia")]
        long Mnesia { get; }
        
        [JsonPropertyName("other_ets")]
        long OtherInMemoryStorage { get; }
        
        [JsonPropertyName("binary")]
        long Binary { get; }
        
        [JsonPropertyName("msg_index")]
        long MessageIndex { get; }
        
        [JsonPropertyName("code")]
        long ByteCode { get; }
        
        [JsonPropertyName("atom")]
        long Atom { get; }
        
        [JsonPropertyName("other_system")]
        long OtherSystem { get; }
        
        [JsonPropertyName("allocated_unused")]
        long AllocatedUnused { get; }
        
        [JsonPropertyName("reserved_unallocated")]
        long ReservedUnallocated { get; }
        
        [JsonPropertyName("strategy")]
        string Strategy { get; }
        
        [JsonPropertyName("total")]
        TotalMemoryInfo Total { get; }
        
        [JsonPropertyName("quorum_queue_procs")]
        long QuorumQueueProcesses { get; }
        
        [JsonPropertyName("quorum_ets")]
        long QuorumInMemoryStorage { get; }
    }
}