namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalMemory :
        MemoryInfo
    {
        public InternalMemory(MemoryInfoImpl obj)
        {
            ConnectionReaders = obj.ConnectionReaders;
            ConnectionWriters = obj.ConnectionWriters;
            ConnectionChannels = obj.ConnectionChannels;
            ConnectionOther = obj.ConnectionOther;
            QueueProcesses = obj.QueueProcesses;
            QueueSlaveProcesses = obj.QueueSlaveProcesses;
            Plugins = obj.Plugins;
            OtherProcesses = obj.OtherProcesses;
            Metrics = obj.Metrics;
            ManagementDatabase = obj.ManagementDatabase;
            Mnesia = obj.Mnesia;
            OtherInMemoryStorage = obj.OtherInMemoryStorage;
            Binary = obj.Binary;
            MessageIndex = obj.MessageIndex;
            ByteCode = obj.ByteCode;
            Atom = obj.Atom;
            OtherSystem = obj.OtherSystem;
            AllocatedUnused = obj.AllocatedUnused;
            ReservedUnallocated = obj.ReservedUnallocated;
            Strategy = obj.Strategy;
            Total = obj.Total.IsNotNull() ? new InternalTotalMemory(obj.Total) : default;
            QuorumQueueProcesses = obj.QuorumQueueProcesses;
            QuorumInMemoryStorage = obj.QuorumInMemoryStorage;
        }

        public long ConnectionReaders { get; }
        public long ConnectionWriters { get; }
        public long ConnectionChannels { get; }
        public long ConnectionOther { get; }
        public long QueueProcesses { get; }
        public long QueueSlaveProcesses { get; }
        public long Plugins { get; }
        public long OtherProcesses { get; }
        public long Metrics { get; }
        public long ManagementDatabase { get; }
        public long Mnesia { get; }
        public long OtherInMemoryStorage { get; }
        public long Binary { get; }
        public long MessageIndex { get; }
        public long ByteCode { get; }
        public long Atom { get; }
        public long OtherSystem { get; }
        public long AllocatedUnused { get; }
        public long ReservedUnallocated { get; }
        public string Strategy { get; }
        public TotalMemoryInfo Total { get; }
        public long QuorumQueueProcesses { get; }
        public long QuorumInMemoryStorage { get; }
    }
}