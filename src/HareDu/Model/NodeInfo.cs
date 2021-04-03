namespace HareDu.Model
{
    using System.Collections.Generic;
    using Internal.Model;

    public interface NodeInfo
    {
        IList<string> Partitions { get; }
        
        string OperatingSystemProcessId { get; }

        ulong TotalFileDescriptors { get; }

        ulong TotalSocketsAvailable { get; }

        ulong MemoryLimit { get; }

        bool MemoryAlarm { get; }

        ulong FreeDiskLimit { get; }

        bool FreeDiskAlarm { get; }

        ulong TotalProcesses { get; }

        RatesMode RatesMode { get; }

        long Uptime { get; }

        int RunQueue { get; }

        string Type { get; }
        
        uint AvailableCoresDetected { get; }

        IList<ExchangeType> ExchangeTypes { get; }

        IList<AuthenticationMechanism> AuthenticationMechanisms { get; }

        IList<Application> Applications { get; }

        IList<NodeContext> Contexts { get; }

        string LogFile { get; }
        
        IList<string> LogFiles { get; }

        string SaslLogFile { get; }

        string DatabaseDirectory { get; }
        
        IList<string> ConfigFiles { get; }

        long NetworkTickTime { get; }

        IList<string> EnabledPlugins { get; }

        string MemoryCalculationStrategy { get; }

        string Name { get; }

        bool IsRunning { get; }

        ulong MemoryUsed { get; }

        Rate MemoryUsageDetails { get; }

        ulong FileDescriptorUsed { get; }

        Rate FileDescriptorUsedDetails { get; }

        ulong SocketsUsed { get; }

        Rate SocketsUsedDetails { get; }

        ulong ProcessesUsed { get; }

        Rate ProcessUsageDetails { get; }

        ulong FreeDiskSpace { get; }

        Rate FreeDiskSpaceDetails { get; }

        ulong NumberOfGarbageCollected { get; }

        Rate GcDetails { get; }

        ulong BytesReclaimedByGarbageCollector { get; }

        Rate ReclaimedBytesFromGCDetails { get; }

        ulong ContextSwitches { get; }

        Rate ContextSwitchDetails { get; }

        ulong TotalIOReads { get; }

        Rate IOReadDetails { get; }

        ulong TotalIOBytesRead { get; }

        Rate IOBytesReadDetails { get; }

        decimal AvgIOReadTime { get; }

        Rate AvgIOReadTimeDetails { get; }

        ulong TotalIOWrites { get; }

        Rate IOWriteDetails { get; }

        ulong TotalIOBytesWritten { get; }

        Rate IOBytesWrittenDetails { get; }

        decimal AvgTimePerIOWrite { get; }

        Rate AvgTimePerIOWriteDetails { get; }

        ulong IOSyncCount { get; }

        Rate IOSyncsDetails { get; }

        decimal AverageIOSyncTime { get; }

        Rate AvgIOSyncTimeDetails { get; }

        ulong IOSeekCount { get; }

        Rate IOSeeksDetails { get; }

        decimal AverageIOSeekTime { get; }

        Rate AvgIOSeekTimeDetails { get; }

        ulong TotalIOReopened { get; }

        Rate IOReopenedDetails { get; }

        ulong TotalMnesiaRamTransactions { get; }

        Rate MnesiaRAMTransactionCountDetails { get; }

        ulong TotalMnesiaDiskTransactions { get; }

        Rate MnesiaDiskTransactionCountDetails { get; }

        ulong TotalMessageStoreReads { get; }

        Rate MessageStoreReadDetails { get; }

        ulong TotalMessageStoreWrites { get; }

        Rate MessageStoreWriteDetails { get; }

        ulong TotalQueueIndexJournalWrites { get; }

        Rate QueueIndexJournalWriteDetails { get; }

        ulong TotalQueueIndexWrites { get; }

        Rate QueueIndexWriteDetails { get; }

        ulong TotalQueueIndexReads { get; }

        Rate QueueIndexReadDetails { get; }

        ulong TotalOpenFileHandleAttempts { get; }

        Rate FileHandleOpenAttemptDetails { get; }

        decimal OpenFileHandleAttemptsAvgTime { get; }

        Rate FileHandleOpenAttemptAvgTimeDetails { get; }

        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
        
        ulong TotalChannelsClosed { get; }
        
        Rate ClosedChannelDetails { get; }

        ulong TotalChannelsCreated { get; }
        
        Rate CreatedChannelDetails { get; }

        ulong TotalConnectionsClosed { get; }
        
        Rate ClosedConnectionDetails { get; }

        ulong TotalConnectionsCreated { get; }
        
        Rate CreatedConnectionDetails { get; }

        ulong TotalQueuesCreated { get; }
        
        Rate CreatedQueueDetails { get; }

        ulong TotalQueuesDeclared { get; }
        
        Rate DeclaredQueueDetails { get; }

        ulong TotalQueuesDeleted { get; }
        
        Rate DeletedQueueDetails { get; }
    }
}