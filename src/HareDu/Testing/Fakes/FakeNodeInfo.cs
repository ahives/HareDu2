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
        public string Type { get; }
        public bool IsRunning { get; }
        public ulong MemoryUsed { get; }
        public MemoryUsageDetails MemoryUsageDetails { get; }
        public ulong FileDescriptorUsed { get; }
        public FileDescriptorUsedDetails FileDescriptorUsedDetails { get; }
        public ulong SocketsUsed { get; }
        public SocketsUsedDetails SocketsUsedDetails { get; }
        public ulong ProcessesUsed { get; }
        public ProcessUsageDetails ProcessUsageDetails { get; }
        public ulong FreeDiskSpace { get; }
        public FreeDiskSpaceDetails FreeDiskSpaceDetails { get; }
        public ulong NumberOfGarbageCollected { get; }
        public GCDetails GcDetails { get; }
        public ulong ReclaimedBytesFromGC { get; }
        public ReclaimedBytesFromGCDetails ReclaimedBytesFromGCDetails { get; }
        public long ContextSwitches { get; }
        public ContextSwitchDetails ContextSwitchDetails { get; }
        public ulong TotalIOReads { get; }
        public IOReadCountDetails IOReadCountDetails { get; }
        public ulong TotalIOBytesRead { get; }
        public IOBytesReadDetails IOReadBytesDetails { get; }
        public ulong AvgIOReadTime { get; }
        public AvgIOReadTimeDetails AvgIOReadTimeDetails { get; }
        public ulong TotalIOWrites { get; }
        public IOWriteDetails IOWriteDetails { get; }
        public ulong TotalIOBytesWritten { get; }
        public IOWriteBytesDetail IOWriteBytesDetail { get; }
        public ulong AvgTimePerIOWrite { get; }
        public AvgTimePerIOWriteDetails AvgTimePerIOWriteDetails { get; }
        public ulong IOSyncCount { get; }
        public IOSyncCountDetails RateOfIOSyncs { get; }
        public decimal AverageIOSyncTime { get; }
        public AvgIOSyncTimeDetails AvgIOSyncTimeDetails { get; }
        public ulong IOSeekCount { get; }
        public IOSeekCountDetails RateOfIOSeeks { get; }
        public ulong AverageIOSeekTime { get; }
        public AvgIOSeekTimeDetails AvgIOSeekTimeDetails { get; }
        public ulong IOReopenCount { get; }
        public IOReopenCountDetails RateOfIOReopened { get; }
        public ulong TotalMnesiaRamTransactions { get; }
        public MnesiaRAMTransactionCountDetails MnesiaRAMTransactionCountDetails { get; }
        public ulong TotalMnesiaDiskTransactions { get; }
        public MnesiaDiskTransactionCountDetails MnesiaDiskTransactionCountDetails { get; }
        public ulong TotalMessageStoreReads { get; }
        public MessageStoreReadCountDetails MessageStoreReadCountDetails { get; }
        public ulong TotalMessageStoreWrites { get; }
        public MessageStoreWriteCountDetails MessageStoreWriteCountDetails { get; }
        public ulong TotalQueueIndexJournalWrites { get; }
        public QueueIndexJournalWriteCountDetails QueueIndexJournalWriteCountDetails { get; }
        public ulong TotalQueueIndexWrites { get; }
        public QueueIndexWriteCountDetails QueueIndexWriteCountDetails { get; }
        public ulong TotalQueueIndexReads { get; }
        public QueueIndexReadCountDetails QueueIndexReadCountDetails { get; }
        public ulong TotalOpenFileHandleAttempts { get; }
        public FileHandleOpenAttemptCountDetails FileHandleOpenAttemptCountDetails { get; }
        public decimal OpenFileHandleAttemptsAvgTime { get; }
        public FileHandleOpenAttemptAvgTimeDetails FileHandleOpenAttemptAvgTimeDetails { get; }
        public GarbageCollectionMetrics GarbageCollectionMetrics { get; }
    }
}