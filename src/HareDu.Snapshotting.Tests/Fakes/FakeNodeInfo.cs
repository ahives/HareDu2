// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using HareDu.Model;

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
        public Rate FileDescriptorUsedDetails { get; }
        public ulong SocketsUsed { get; }
        public Rate SocketsUsedDetails { get; }
        public ulong ProcessesUsed { get; }
        public ProcessUsageDetails ProcessUsageDetails { get; }
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