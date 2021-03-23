namespace HareDu.Model
{
    public interface MemoryInfo
    {
        long ConnectionReaders { get; }
        
        long ConnectionWriters { get; }
        
        long ConnectionChannels { get; }
        
        long ConnectionOther { get; }
        
        long QueueProcesses { get; }
        
        long QueueSlaveProcesses { get; }
        
        long Plugins { get; }
        
        long OtherProcesses { get; }
        
        long Metrics { get; }
        
        long ManagementDatabase { get; }
        
        long Mnesia { get; }
        
        long OtherInMemoryStorage { get; }
        
        long Binary { get; }
        
        long MessageIndex { get; }
        
        long ByteCode { get; }
        
        long Atom { get; }
        
        long OtherSystem { get; }
        
        long AllocatedUnused { get; }
        
        long ReservedUnallocated { get; }
        
        string Strategy { get; }
        
        TotalMemoryInfo Total { get; }
        
        long QuorumQueueProcesses { get; }
        
        long QuorumInMemoryStorage { get; }
    }
}