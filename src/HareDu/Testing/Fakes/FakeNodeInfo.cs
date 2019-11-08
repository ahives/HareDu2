// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Testing.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class FakeNodeInfo :
        NodeInfo
    {
        public FakeNodeInfo()
        {
            Partitions = GetPartitions().ToList();
            MemoryAlarm = true;
            MemoryLimit = 723746434;
            OperatingSystemProcessId = "OS123";
            TotalFileDescriptors = 8923747343434;
            TotalSocketsAvailable = 8186263662;
            FreeDiskLimit = 8928739432;
            FreeDiskAlarm = true;
            TotalProcesses = 7234364;
            AvailableCoresDetected = 8;
            IsRunning = true;
            MemoryUsed = 7826871;
            FreeDiskSpace = 8266864;
            FileDescriptorUsed = 9203797;
            SocketsUsed = 8298347;
            ProcessesUsed = 9199849;
            FreeDiskSpace = 7265368234;
            TotalOpenFileHandleAttempts = 356446478;
            TotalIOWrites = 36478608776;
            TotalIOReads = 892793874982;
            TotalMessageStoreReads = 9097887;
            TotalMessageStoreWrites = 776788733;
            TotalIOBytesRead = 78738764;
            TotalIOBytesWritten = 728364283;
        }

        IEnumerable<string> GetPartitions()
        {
            yield return "partition1";
            yield return "partition2";
            yield return "partition3";
            yield return "partition4";
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
        public FileDescriptorUsedDetails FileDescriptorUsedDetails { get; }
        public ulong SocketsUsed { get; }
        public Rate SocketsUsedDetails { get; }
        public ulong ProcessesUsed { get; }
        public ProcessUsageDetails ProcessUsageDetails { get; }
        public ulong FreeDiskSpace { get; }
        public Rate FreeDiskSpaceDetails { get; }
        public ulong NumberOfGarbageCollected { get; }
        public GCDetails GcDetails { get; }
        public ulong ReclaimedBytesFromGC { get; }
        public Rate ReclaimedBytesFromGCDetails { get; }
        public ulong ContextSwitches { get; }
        public Rate ContextSwitchDetails { get; }
        public ulong TotalIOReads { get; }
        public Rate IOReadCountDetails { get; }
        public ulong TotalIOBytesRead { get; }
        public Rate IOReadBytesDetails { get; }
        public ulong AvgIOReadTime { get; }
        public Rate AvgIOReadTimeDetails { get; }
        public ulong TotalIOWrites { get; }
        public Rate IOWriteDetails { get; }
        public ulong TotalIOBytesWritten { get; }
        public Rate IOWriteBytesDetail { get; }
        public ulong AvgTimePerIOWrite { get; }
        public Rate AvgTimePerIOWriteDetails { get; }
        public ulong IOSyncCount { get; }
        public Rate RateOfIOSyncs { get; }
        public decimal AverageIOSyncTime { get; }
        public Rate AvgIOSyncTimeDetails { get; }
        public ulong IOSeekCount { get; }
        public Rate RateOfIOSeeks { get; }
        public ulong AverageIOSeekTime { get; }
        public Rate AvgIOSeekTimeDetails { get; }
        public ulong IOReopenCount { get; }
        public Rate RateOfIOReopened { get; }
        public ulong TotalMnesiaRamTransactions { get; }
        public Rate MnesiaRAMTransactionCountDetails { get; }
        public ulong TotalMnesiaDiskTransactions { get; }
        public Rate MnesiaDiskTransactionCountDetails { get; }
        public ulong TotalMessageStoreReads { get; }
        public Rate MessageStoreReadCountDetails { get; }
        public ulong TotalMessageStoreWrites { get; }
        public Rate MessageStoreWriteCountDetails { get; }
        public ulong TotalQueueIndexJournalWrites { get; }
        public Rate QueueIndexJournalWriteCountDetails { get; }
        public ulong TotalQueueIndexWrites { get; }
        public Rate QueueIndexWriteCountDetails { get; }
        public ulong TotalQueueIndexReads { get; }
        public Rate QueueIndexReadCountDetails { get; }
        public ulong TotalOpenFileHandleAttempts { get; }
        public Rate FileHandleOpenAttemptCountDetails { get; }
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