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
namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_nodes()
        {
            var container = GetContainerBuilder("TestData/NodeInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll();

            Assert.IsTrue(result.HasData);
            Assert.IsFalse(result.HasFaulted);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data[0]);
            Assert.IsNotNull(result.Data[0]);
            Assert.AreEqual(32, result.Data[0].Applications.Count);
            Assert.AreEqual(1, result.Data[0].Contexts.Count);
            Assert.AreEqual("RabbitMQ Management", result.Data[0].Contexts[0].Description);
            Assert.AreEqual("/", result.Data[0].Contexts[0].Path);
            Assert.AreEqual("15672", result.Data[0].Contexts[0].Port);
            Assert.AreEqual("rabbit@localhost", result.Data[0].Name);
            Assert.AreEqual(2, result.Data[0].Partitions.Count);
            Assert.AreEqual("node1", result.Data[0].Partitions[0]);
            Assert.AreEqual(737892679, result.Data[0].Uptime);
            Assert.IsNotNull(result.Data[0].AuthenticationMechanisms);
            Assert.AreEqual(3, result.Data[0].AuthenticationMechanisms.Count);
            Assert.AreEqual("RABBIT-CR-DEMO", result.Data[0].AuthenticationMechanisms[0].Name);
            Assert.AreEqual("RabbitMQ Demo challenge-response authentication mechanism", result.Data[0].AuthenticationMechanisms[0].Description);
            Assert.IsFalse(result.Data[0].AuthenticationMechanisms[0].IsEnabled);
            Assert.AreEqual(2, result.Data[0].LogFiles.Count);
            Assert.AreEqual("/usr/local/var/log/rabbitmq/rabbit@localhost.log", result.Data[0].LogFiles[0]);
            Assert.AreEqual(19886066, result.Data[0].ContextSwitches);
            Assert.IsNotNull(result.Data[0].ContextSwitchDetails);
            Assert.AreEqual(32.4, result.Data[0].ContextSwitchDetails?.Rate);
            Assert.AreEqual("/usr/local/var/lib/rabbitmq/mnesia/rabbit@localhost", result.Data[0].DatabaseDirectory);
            Assert.AreEqual(5, result.Data[0].EnabledPlugins.Count);
            Assert.AreEqual("rabbitmq_management", result.Data[0].EnabledPlugins[0]);
            Assert.AreEqual(4, result.Data[0].ExchangeTypes.Count);
            Assert.AreEqual("headers", result.Data[0].ExchangeTypes[0].Name);
            Assert.AreEqual("AMQP headers exchange, as per the AMQP specification", result.Data[0].ExchangeTypes[0].Description);
            Assert.IsTrue(result.Data[0].ExchangeTypes[0].IsEnabled);
            Assert.IsNotNull(result.Data[0].GcDetails);
            Assert.AreEqual(6.0, result.Data[0].GcDetails?.Rate);
            Assert.AreEqual(4815693, result.Data[0].NumberOfGarbageCollected);
            Assert.IsTrue(result.Data[0].IsRunning);
            Assert.IsFalse(result.Data[0].MemoryAlarm);
            Assert.AreEqual(429496729, result.Data[0].MemoryLimit);
            Assert.AreEqual(11784192, result.Data[0].MemoryUsed);
            Assert.IsNotNull(result.Data[0].MemoryUsageDetails);
            Assert.AreEqual(20480.0, result.Data[0].MemoryUsageDetails?.Rate);
            Assert.AreEqual(1, result.Data[0].SocketsUsed);
            Assert.IsNotNull(result.Data[0].SocketsUsedDetails);
            Assert.AreEqual(0.0, result.Data[0].SocketsUsedDetails?.Rate);
            Assert.AreEqual(138, result.Data[0].TotalSocketsAvailable);
            Assert.AreEqual(598, result.Data[0].ProcessesUsed);
            Assert.IsNotNull(result.Data[0].ProcessUsageDetails);
            Assert.AreEqual(0.0, result.Data[0].ProcessUsageDetails?.Rate);
            Assert.AreEqual(1048576, result.Data[0].TotalProcesses);
            Assert.AreEqual("1864", result.Data[0].OperatingSystemProcessId);
            Assert.AreEqual("basic", result.Data[0].RatesMode);
            Assert.AreEqual(4, result.Data[0].AvailableCoresDetected);
            Assert.IsFalse(result.Data[0].FreeDiskAlarm);
            Assert.AreEqual(50000000, result.Data[0].FreeDiskLimit);
            Assert.AreEqual(0, result.Data[0].FreeDiskSpace);
            Assert.IsNotNull(result.Data[0].FreeDiskSpaceDetails);
            Assert.AreEqual(0.0, result.Data[0].FreeDiskSpaceDetails?.Rate);
            Assert.AreEqual(0, result.Data[0].FileDescriptorUsed);
            Assert.IsNotNull(result.Data[0].FileDescriptorUsedDetails);
            Assert.AreEqual(0.0, result.Data[0].FileDescriptorUsedDetails?.Rate);
            Assert.AreEqual(256, result.Data[0].TotalFileDescriptors);
            Assert.AreEqual(60, result.Data[0].NetworkTickTime);
            Assert.AreEqual(11, result.Data[0].TotalIOReads);
            Assert.IsNotNull(result.Data[0].IOReadCountDetails);
            Assert.AreEqual(0.0, result.Data[0].IOReadCountDetails?.Rate);
            Assert.AreEqual(932, result.Data[0].TotalIOWrites);
            Assert.IsNotNull(result.Data[0].IOWriteDetails);
            Assert.AreEqual(0.0, result.Data[0].IOWriteDetails?.Rate);
            Assert.AreEqual(0, result.Data[0].TotalMessageStoreReads);
            Assert.IsNotNull(result.Data[0].MessageStoreReadCountDetails);
            Assert.AreEqual(0.0, result.Data[0].MessageStoreReadCountDetails?.Rate);
            Assert.AreEqual(0, result.Data[0].TotalMessageStoreWrites);
            Assert.IsNotNull(result.Data[0].MessageStoreReadCountDetails);
            Assert.AreEqual(0.0, result.Data[0].MessageStoreReadCountDetails?.Rate);
            Assert.AreEqual(33, result.Data[0].TotalMnesiaDiskTransactions);
            Assert.IsNotNull(result.Data[0].MnesiaDiskTransactionCountDetails);
            Assert.AreEqual(0.0, result.Data[0].MnesiaDiskTransactionCountDetails?.Rate);
            Assert.AreEqual(1278, result.Data[0].TotalMnesiaRamTransactions);
            Assert.IsNotNull(result.Data[0].MnesiaRAMTransactionCountDetails);
            Assert.AreEqual(0.0, result.Data[0].MnesiaRAMTransactionCountDetails?.Rate);
            Assert.AreEqual(5, result.Data[0].TotalQueueIndexReads);
            Assert.IsNotNull(result.Data[0].QueueIndexReadCountDetails);
            Assert.AreEqual(0.0, result.Data[0].QueueIndexReadCountDetails?.Rate);
            Assert.AreEqual(43, result.Data[0].TotalQueueIndexWrites);
            Assert.IsNotNull(result.Data[0].QueueIndexWriteCountDetails);
            Assert.AreEqual(0.0, result.Data[0].QueueIndexWriteCountDetails?.Rate);
            Assert.AreEqual(44904556, result.Data[0].TotalIOBytesRead);
            Assert.IsNotNull(result.Data[0].IOReadBytesDetails);
            Assert.AreEqual(0.0, result.Data[0].IOReadBytesDetails?.Rate);
            Assert.AreEqual(574531094, result.Data[0].TotalIOBytesWritten);
            Assert.IsNotNull(result.Data[0].IOWriteBytesDetail);
            Assert.AreEqual(0.0, result.Data[0].IOWriteBytesDetail?.Rate);
            Assert.AreEqual(901142, result.Data[0].TotalOpenFileHandleAttempts);
            Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptCountDetails);
            Assert.AreEqual(0.0, result.Data[0].FileHandleOpenAttemptCountDetails?.Rate);
//            Assert.AreEqual(0.0036650927378814886, result.Data[0].OpenFileHandleAttemptsAvgTime);
            Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptAvgTimeDetails);
            Assert.AreEqual(0.0, result.Data[0].FileHandleOpenAttemptAvgTimeDetails?.Rate);
            Assert.AreEqual(15, result.Data[0].TotalConnectionsCreated);
            Assert.IsNotNull(result.Data[0].CreatedConnectionDetails);
            Assert.AreEqual(0.0, result.Data[0].CreatedConnectionDetails?.Rate);
            Assert.AreEqual(14, result.Data[0].TotalConnectionsClosed);
            Assert.IsNotNull(result.Data[0].ClosedConnectionDetails);
            Assert.AreEqual(0.0, result.Data[0].ClosedConnectionDetails?.Rate);
            Assert.AreEqual(75, result.Data[0].TotalChannelsCreated);
            Assert.IsNotNull(result.Data[0].CreatedChannelDetails);
            Assert.AreEqual(0.0, result.Data[0].CreatedChannelDetails?.Rate);
            Assert.AreEqual(73, result.Data[0].TotalChannelsClosed);
            Assert.IsNotNull(result.Data[0].ClosedChannelDetails);
            Assert.AreEqual(0.0, result.Data[0].ClosedChannelDetails?.Rate);
            Assert.AreEqual(11, result.Data[0].TotalQueuesDeclared);
            Assert.IsNotNull(result.Data[0].DeclaredQueueDetails);
            Assert.AreEqual(0.0, result.Data[0].DeclaredQueueDetails?.Rate);
            Assert.AreEqual(9, result.Data[0].TotalQueuesCreated);
            Assert.IsNotNull(result.Data[0].CreatedQueueDetails);
            Assert.AreEqual(0.0, result.Data[0].CreatedQueueDetails?.Rate);
            Assert.AreEqual(7, result.Data[0].TotalQueuesDeleted);
            Assert.IsNotNull(result.Data[0].DeletedQueueDetails);
            Assert.AreEqual(0.0, result.Data[0].DeletedQueueDetails?.Rate);
//            Assert.AreEqual(0.8947999999999999, result.Data[0].AverageIOSeekTime);
            Assert.IsNotNull(result.Data[0].AvgIOSeekTimeDetails);
            Assert.AreEqual(0.0, result.Data[0].AvgIOSeekTimeDetails?.Rate);
            Assert.AreEqual(80, result.Data[0].IOSeekCount);
            Assert.IsNotNull(result.Data[0].RateOfIOSeeks);
            Assert.AreEqual(0.0, result.Data[0].RateOfIOSeeks?.Rate);
//            Assert.AreEqual(8.185064377682403, result.Data[0].AverageIOSyncTime);
            Assert.IsNotNull(result.Data[0].AvgIOSyncTimeDetails);
            Assert.AreEqual(0.0, result.Data[0].AvgIOSyncTimeDetails?.Rate);
            Assert.AreEqual(932, result.Data[0].IOSyncCount);
            Assert.IsNotNull(result.Data[0].RateOfIOSyncs);
            Assert.AreEqual(0.0, result.Data[0].RateOfIOSyncs?.Rate);
            Assert.AreEqual(114844443912, result.Data[0].BytesReclaimedByGarbageCollector);
            Assert.IsNotNull(result.Data[0].ReclaimedBytesFromGCDetails);
            Assert.AreEqual(147054.4, result.Data[0].ReclaimedBytesFromGCDetails?.Rate);
            Assert.AreEqual("disc", result.Data[0].Type);
        }
    }
}