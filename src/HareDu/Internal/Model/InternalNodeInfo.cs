namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalNodeInfo :
        NodeInfo
    {
        public InternalNodeInfo(NodeInfoImpl obj)
        {
            Partitions = obj.Partitions;
            OperatingSystemProcessId = obj.OperatingSystemProcessId;
            TotalFileDescriptors = obj.TotalFileDescriptors;
            TotalSocketsAvailable = obj.TotalSocketsAvailable;
            MemoryLimit = obj.MemoryLimit;
            MemoryAlarm = obj.MemoryAlarm;
            FreeDiskLimit = obj.FreeDiskLimit;
            FreeDiskAlarm = obj.FreeDiskAlarm;
            TotalProcesses = obj.TotalProcesses;
            RatesMode = obj.RatesMode;
            Uptime = obj.Uptime;
            RunQueue = obj.RunQueue;
            Type = obj.Type;
            AvailableCoresDetected = obj.AvailableCoresDetected;

            List<AuthenticationMechanism> MapAuthenticationMechanisms(IList<AuthenticationMechanismImpl> list)
            {
                if (list.IsNull())
                    return default;
                
                var authMechanisms = new List<AuthenticationMechanism>();
                foreach (var authMechanism in list)
                    authMechanisms.Add(new InternalAuthenticationMechanism(authMechanism));

                return authMechanisms;
            }

            AuthenticationMechanisms = MapAuthenticationMechanisms(obj.AuthenticationMechanisms);

            List<ExchangeType> MapExchangeTypes(IList<ExchangeTypeImpl> list)
            {
                if (list.IsNull())
                    return default;
                
                var exchangeTypes = new List<ExchangeType>();
                foreach (var exchangeType in list)
                    exchangeTypes.Add(new InternalExchangeType(exchangeType));

                return exchangeTypes;
            }

            ExchangeTypes = MapExchangeTypes(obj.ExchangeTypes);

            List<Application> MapApplications(IList<ApplicationImpl> list)
            {
                if (list.IsNull())
                    return default;
                
                var applications = new List<Application>();
                foreach (var application in list)
                    applications.Add(new InternalApplication(application));

                return applications;
            }

            Applications = MapApplications(obj.Applications);

            List<NodeContext> MapContexts(IList<NodeContextImpl> list)
            {
                if (list.IsNull())
                    return default;
                
                var contexts = new List<NodeContext>();
                foreach (var context in list)
                    contexts.Add(new InternalContext(context));

                return contexts;
            }

            Contexts = MapContexts(obj.Contexts);
            
            LogFile = obj.LogFile;
            LogFiles = obj.LogFiles;
            SaslLogFile = obj.SaslLogFile;
            DatabaseDirectory = obj.DatabaseDirectory;
            ConfigFiles = obj.ConfigFiles;
            NetworkTickTime = obj.NetworkTickTime;
            EnabledPlugins = obj.EnabledPlugins;
            MemoryCalculationStrategy = obj.MemoryCalculationStrategy;
            Name = obj.Name;
            IsRunning = obj.IsRunning;
            MemoryUsed = obj.MemoryUsed;
            MemoryUsageDetails = obj.MemoryUsageDetails.IsNotNull() ? new InternalRateImpl(obj.MemoryUsageDetails) : default;
            FileDescriptorUsed = obj.FileDescriptorUsed;
            FileDescriptorUsedDetails = obj.FileDescriptorUsedDetails.IsNotNull() ? new InternalRateImpl(obj.FileDescriptorUsedDetails) : default;
            SocketsUsed = obj.SocketsUsed;
            SocketsUsedDetails = obj.SocketsUsedDetails.IsNotNull() ? new InternalRateImpl(obj.SocketsUsedDetails) : default;
            ProcessesUsed = obj.ProcessesUsed;
            ProcessUsageDetails = obj.ProcessUsageDetails.IsNotNull() ? new InternalRateImpl(obj.ProcessUsageDetails) : default;
            FreeDiskSpace = obj.FreeDiskSpace;
            FreeDiskSpaceDetails = obj.FreeDiskSpaceDetails.IsNotNull() ? new InternalRateImpl(obj.FreeDiskSpaceDetails) : default;
            NumberOfGarbageCollected = obj.NumberOfGarbageCollected;
            GcDetails = obj.GcDetails.IsNotNull() ? new InternalRateImpl(obj.GcDetails) : default;
            BytesReclaimedByGarbageCollector = obj.BytesReclaimedByGarbageCollector;
            ReclaimedBytesFromGCDetails = obj.ReclaimedBytesFromGCDetails.IsNotNull() ? new InternalRateImpl(obj.ReclaimedBytesFromGCDetails) : default;
            ContextSwitches = obj.ContextSwitches;
            ContextSwitchDetails = obj.ContextSwitchDetails.IsNotNull() ? new InternalRateImpl(obj.ContextSwitchDetails) : default;
            TotalIOReads = obj.TotalIOReads;
            IOReadDetails = obj.IOReadDetails.IsNotNull() ? new InternalRateImpl(obj.IOReadDetails) : default;
            TotalIOBytesRead = obj.TotalIOBytesRead;
            IOBytesReadDetails = obj.IOBytesReadDetails.IsNotNull() ? new InternalRateImpl(obj.IOBytesReadDetails) : default;
            AvgIOReadTime = obj.AvgIOReadTime;
            AvgIOReadTimeDetails = obj.AvgIOReadTimeDetails.IsNotNull() ? new InternalRateImpl(obj.AvgIOReadTimeDetails) : default;
            TotalIOWrites = obj.TotalIOWrites;
            IOWriteDetails = obj.IOWriteDetails.IsNotNull() ? new InternalRateImpl(obj.IOWriteDetails) : default;
            TotalIOBytesWritten = obj.TotalIOBytesWritten;
            IOBytesWrittenDetails = obj.IOBytesWrittenDetails.IsNotNull() ? new InternalRateImpl(obj.IOBytesWrittenDetails) : default;
            AvgTimePerIOWrite = obj.AvgTimePerIOWrite;
            AvgTimePerIOWriteDetails = obj.AvgTimePerIOWriteDetails.IsNotNull() ? new InternalRateImpl(obj.AvgTimePerIOWriteDetails) : default;
            IOSyncCount = obj.IOSyncCount;
            IOSyncsDetails = obj.IOSyncsDetails.IsNotNull() ? new InternalRateImpl(obj.IOSyncsDetails) : default;
            AverageIOSyncTime = obj.AverageIOSyncTime;
            AvgIOSyncTimeDetails = obj.AvgIOSyncTimeDetails.IsNotNull() ? new InternalRateImpl(obj.AvgIOSyncTimeDetails) : default;
            IOSeekCount = obj.IOSeekCount;
            IOSeeksDetails = obj.IOSeeksDetails.IsNotNull() ? new InternalRateImpl(obj.IOSeeksDetails) : default;
            AverageIOSeekTime = obj.AverageIOSeekTime;
            AvgIOSeekTimeDetails = obj.AvgIOSeekTimeDetails.IsNotNull() ? new InternalRateImpl(obj.AvgIOSeekTimeDetails) : default;
            TotalIOReopened = obj.TotalIOReopened;
            IOReopenedDetails = obj.IOReopenedDetails.IsNotNull() ? new InternalRateImpl(obj.IOReopenedDetails) : default;
            TotalMnesiaRamTransactions = obj.TotalMnesiaRamTransactions;
            MnesiaRAMTransactionCountDetails = obj.MnesiaRAMTransactionCountDetails.IsNotNull() ? new InternalRateImpl(obj.MnesiaRAMTransactionCountDetails) : default;
            TotalMnesiaDiskTransactions = obj.TotalMnesiaDiskTransactions;
            MnesiaDiskTransactionCountDetails = obj.MnesiaDiskTransactionCountDetails.IsNotNull() ? new InternalRateImpl(obj.MnesiaDiskTransactionCountDetails) : default;
            TotalMessageStoreReads = obj.TotalMessageStoreReads;
            MessageStoreReadDetails = obj.MessageStoreReadDetails.IsNotNull() ? new InternalRateImpl(obj.MessageStoreReadDetails) : default;
            TotalMessageStoreWrites = obj.TotalMessageStoreWrites;
            MessageStoreWriteDetails = obj.MessageStoreWriteDetails.IsNotNull() ? new InternalRateImpl(obj.MessageStoreWriteDetails) : default;
            TotalQueueIndexJournalWrites = obj.TotalQueueIndexJournalWrites;
            QueueIndexJournalWriteDetails = obj.QueueIndexJournalWriteDetails.IsNotNull() ? new InternalRateImpl(obj.QueueIndexJournalWriteDetails) : default;
            TotalQueueIndexWrites = obj.TotalQueueIndexWrites;
            QueueIndexWriteDetails = obj.QueueIndexWriteDetails.IsNotNull() ? new InternalRateImpl(obj.QueueIndexWriteDetails) : default;
            TotalQueueIndexReads = obj.TotalQueueIndexReads;
            QueueIndexReadDetails = obj.QueueIndexReadDetails.IsNotNull() ? new InternalRateImpl(obj.QueueIndexReadDetails) : default;
            TotalOpenFileHandleAttempts = obj.TotalOpenFileHandleAttempts;
            FileHandleOpenAttemptDetails = obj.FileHandleOpenAttemptDetails.IsNotNull() ? new InternalRateImpl(obj.FileHandleOpenAttemptDetails) : default;
            OpenFileHandleAttemptsAvgTime = obj.OpenFileHandleAttemptsAvgTime;
            FileHandleOpenAttemptAvgTimeDetails = obj.FileHandleOpenAttemptAvgTimeDetails.IsNotNull() ? new InternalRateImpl(obj.FileHandleOpenAttemptAvgTimeDetails) : default;
            GarbageCollectionMetrics = obj.GarbageCollectionMetrics.IsNotNull() ? new InternalGarbageCollectionMetrics(obj.GarbageCollectionMetrics) : default;
            TotalChannelsClosed = obj.TotalChannelsClosed;
            ClosedChannelDetails = obj.ClosedChannelDetails.IsNotNull() ? new InternalRateImpl(obj.ClosedChannelDetails) : default;
            TotalChannelsCreated = obj.TotalChannelsCreated;
            CreatedChannelDetails = obj.CreatedChannelDetails.IsNotNull() ? new InternalRateImpl(obj.CreatedChannelDetails) : default;
            TotalConnectionsClosed = obj.TotalConnectionsClosed;
            ClosedConnectionDetails = obj.ClosedConnectionDetails.IsNotNull() ? new InternalRateImpl(obj.ClosedConnectionDetails) : default;
            TotalConnectionsCreated = obj.TotalConnectionsCreated;
            CreatedConnectionDetails = obj.CreatedConnectionDetails.IsNotNull() ? new InternalRateImpl(obj.CreatedConnectionDetails) : default;
            TotalQueuesCreated = obj.TotalQueuesCreated;
            CreatedQueueDetails = obj.CreatedQueueDetails.IsNotNull() ? new InternalRateImpl(obj.CreatedQueueDetails) : default;
            TotalQueuesDeclared = obj.TotalQueuesDeclared;
            DeclaredQueueDetails = obj.DeclaredQueueDetails.IsNotNull() ? new InternalRateImpl(obj.DeclaredQueueDetails) : default;
            TotalQueuesDeleted = obj.TotalQueuesDeleted;
            DeletedQueueDetails = obj.DeletedQueueDetails.IsNotNull() ? new InternalRateImpl(obj.DeletedQueueDetails) : default;
        }

        public IList<string> Partitions { get; }
        public string OperatingSystemProcessId { get; }
        public ulong TotalFileDescriptors { get; }
        public ulong TotalSocketsAvailable { get; }
        public ulong MemoryLimit { get; }
        public bool MemoryAlarm { get; }
        public ulong FreeDiskLimit { get; }
        public bool FreeDiskAlarm { get; }
        public ulong TotalProcesses { get; }
        public string RatesMode { get; }
        public long Uptime { get; }
        public int RunQueue { get; }
        public string Type { get; }
        public uint AvailableCoresDetected { get; }
        public IList<ExchangeType> ExchangeTypes { get; }
        public IList<AuthenticationMechanism> AuthenticationMechanisms { get; }
        public IList<Application> Applications { get; }
        public IList<NodeContext> Contexts { get; }
        public string LogFile { get; }
        public IList<string> LogFiles { get; }
        public string SaslLogFile { get; }
        public string DatabaseDirectory { get; }
        public IList<string> ConfigFiles { get; }
        public long NetworkTickTime { get; }
        public IList<string> EnabledPlugins { get; }
        public string MemoryCalculationStrategy { get; }
        public string Name { get; }
        public bool IsRunning { get; }
        public ulong MemoryUsed { get; }
        public Rate MemoryUsageDetails { get; }
        public ulong FileDescriptorUsed { get; }
        public Rate FileDescriptorUsedDetails { get; }
        public ulong SocketsUsed { get; }
        public Rate SocketsUsedDetails { get; }
        public ulong ProcessesUsed { get; }
        public Rate ProcessUsageDetails { get; }
        public ulong FreeDiskSpace { get; }
        public Rate FreeDiskSpaceDetails { get; }
        public ulong NumberOfGarbageCollected { get; }
        public Rate GcDetails { get; }
        public ulong BytesReclaimedByGarbageCollector { get; }
        public Rate ReclaimedBytesFromGCDetails { get; }
        public ulong ContextSwitches { get; }
        public Rate ContextSwitchDetails { get; }
        public ulong TotalIOReads { get; }
        public Rate IOReadDetails { get; }
        public ulong TotalIOBytesRead { get; }
        public Rate IOBytesReadDetails { get; }
        public decimal AvgIOReadTime { get; }
        public Rate AvgIOReadTimeDetails { get; }
        public ulong TotalIOWrites { get; }
        public Rate IOWriteDetails { get; }
        public ulong TotalIOBytesWritten { get; }
        public Rate IOBytesWrittenDetails { get; }
        public decimal AvgTimePerIOWrite { get; }
        public Rate AvgTimePerIOWriteDetails { get; }
        public ulong IOSyncCount { get; }
        public Rate IOSyncsDetails { get; }
        public decimal AverageIOSyncTime { get; }
        public Rate AvgIOSyncTimeDetails { get; }
        public ulong IOSeekCount { get; }
        public Rate IOSeeksDetails { get; }
        public decimal AverageIOSeekTime { get; }
        public Rate AvgIOSeekTimeDetails { get; }
        public ulong TotalIOReopened { get; }
        public Rate IOReopenedDetails { get; }
        public ulong TotalMnesiaRamTransactions { get; }
        public Rate MnesiaRAMTransactionCountDetails { get; }
        public ulong TotalMnesiaDiskTransactions { get; }
        public Rate MnesiaDiskTransactionCountDetails { get; }
        public ulong TotalMessageStoreReads { get; }
        public Rate MessageStoreReadDetails { get; }
        public ulong TotalMessageStoreWrites { get; }
        public Rate MessageStoreWriteDetails { get; }
        public ulong TotalQueueIndexJournalWrites { get; }
        public Rate QueueIndexJournalWriteDetails { get; }
        public ulong TotalQueueIndexWrites { get; }
        public Rate QueueIndexWriteDetails { get; }
        public ulong TotalQueueIndexReads { get; }
        public Rate QueueIndexReadDetails { get; }
        public ulong TotalOpenFileHandleAttempts { get; }
        public Rate FileHandleOpenAttemptDetails { get; }
        public decimal OpenFileHandleAttemptsAvgTime { get; }
        public Rate FileHandleOpenAttemptAvgTimeDetails { get; }
        public GarbageCollectionMetrics GarbageCollectionMetrics { get; }
        public ulong TotalChannelsClosed { get; }
        public Rate ClosedChannelDetails { get; }
        public ulong TotalChannelsCreated { get; }
        public Rate CreatedChannelDetails { get; }
        public ulong TotalConnectionsClosed { get; }
        public Rate ClosedConnectionDetails { get; }
        public ulong TotalConnectionsCreated { get; }
        public Rate CreatedConnectionDetails { get; }
        public ulong TotalQueuesCreated { get; }
        public Rate CreatedQueueDetails { get; }
        public ulong TotalQueuesDeclared { get; }
        public Rate DeclaredQueueDetails { get; }
        public ulong TotalQueuesDeleted { get; }
        public Rate DeletedQueueDetails { get; }
    }
}