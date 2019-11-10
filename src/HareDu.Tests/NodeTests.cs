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
    using Shouldly;

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

            result.HasData.ShouldBeTrue();
            result.HasFaulted.ShouldBeFalse();
            result.Data.ShouldNotBeNull();
            result.Data[0].ShouldNotBeNull();
            result.Data[0].Applications.Count.ShouldBe(32);
            result.Data[0].Contexts.Count.ShouldBe(1);
            result.Data[0].Contexts[0].Description.ShouldBe("RabbitMQ Management");
            result.Data[0].Contexts[0].Path.ShouldBe("/");
            result.Data[0].Contexts[0].Port.ShouldBe("15672");
            result.Data[0].Name.ShouldBe("rabbit@localhost");
            result.Data[0].Partitions.Count.ShouldBe(2);
            result.Data[0].Partitions[0].ShouldBe("node1");
            result.Data[0].Uptime.ShouldBe(737892679);
            result.Data[0].AuthenticationMechanisms.ShouldNotBeNull();
            result.Data[0].AuthenticationMechanisms.Count.ShouldBe(3);
            result.Data[0].AuthenticationMechanisms[0].Name.ShouldBe("RABBIT-CR-DEMO");
            result.Data[0].AuthenticationMechanisms[0].Description.ShouldBe("RabbitMQ Demo challenge-response authentication mechanism");
            result.Data[0].AuthenticationMechanisms[0].IsEnabled.ShouldBeFalse();
            result.Data[0].LogFiles.Count.ShouldBe(2);
            result.Data[0].LogFiles[0].ShouldBe("/usr/local/var/log/rabbitmq/rabbit@localhost.log");
            result.Data[0].ContextSwitches.ShouldBe<ulong>(19886066);
            result.Data[0].ContextSwitchDetails.ShouldNotBeNull();
            result.Data[0].ContextSwitchDetails?.Rate.ShouldBe(32.4M);
            result.Data[0].DatabaseDirectory.ShouldBe("/usr/local/var/lib/rabbitmq/mnesia/rabbit@localhost");
            result.Data[0].EnabledPlugins.Count.ShouldBe(5);
            result.Data[0].EnabledPlugins[0].ShouldBe("rabbitmq_management");
            result.Data[0].ExchangeTypes.Count.ShouldBe(4);
            result.Data[0].ExchangeTypes[0].Name.ShouldBe("headers");
            result.Data[0].ExchangeTypes[0].Description.ShouldBe("AMQP headers exchange, as per the AMQP specification");
            result.Data[0].ExchangeTypes[0].IsEnabled.ShouldBeTrue();
            result.Data[0].GcDetails.ShouldNotBeNull();
            result.Data[0].GcDetails?.Rate.ShouldBe(6.0M);
            result.Data[0].NumberOfGarbageCollected.ShouldBe<ulong>(4815693);
            result.Data[0].IsRunning.ShouldBeTrue();
            result.Data[0].MemoryAlarm.ShouldBeFalse();
            result.Data[0].MemoryLimit.ShouldBe<ulong>(429496729);
            result.Data[0].MemoryUsed.ShouldBe<ulong>(11784192);
            result.Data[0].MemoryUsageDetails.ShouldNotBeNull();
            result.Data[0].MemoryUsageDetails?.Rate.ShouldBe(20480.0M);
            result.Data[0].SocketsUsed.ShouldBe<ulong>(1);
            result.Data[0].SocketsUsedDetails.ShouldNotBeNull();
            result.Data[0].SocketsUsedDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalSocketsAvailable.ShouldBe<ulong>(138);
            result.Data[0].ProcessesUsed.ShouldBe<ulong>(598);
            result.Data[0].ProcessUsageDetails.ShouldNotBeNull();
            result.Data[0].ProcessUsageDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalProcesses.ShouldBe<ulong>(1048576);
            result.Data[0].OperatingSystemProcessId.ShouldBe("1864");
            result.Data[0].RatesMode.ShouldBe("basic");
            result.Data[0].AvailableCoresDetected.ShouldBe<uint>(4);
            result.Data[0].FreeDiskAlarm.ShouldBeFalse();
            result.Data[0].FreeDiskLimit.ShouldBe<ulong>(50000000);
            result.Data[0].FreeDiskSpace.ShouldBe<ulong>(0);
            result.Data[0].FreeDiskSpaceDetails.ShouldNotBeNull();
            result.Data[0].FreeDiskSpaceDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].FileDescriptorUsed.ShouldBe<ulong>(0);
            result.Data[0].FileDescriptorUsedDetails.ShouldNotBeNull();
            result.Data[0].FileDescriptorUsedDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalFileDescriptors.ShouldBe<ulong>(256);
            result.Data[0].NetworkTickTime.ShouldBe(60);
            result.Data[0].TotalIOReads.ShouldBe<ulong>(11);
            result.Data[0].IOReadCountDetails.ShouldNotBeNull();
            result.Data[0].IOReadCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOWrites.ShouldBe<ulong>(932);
            result.Data[0].IOWriteDetails.ShouldNotBeNull();
            result.Data[0].IOWriteDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMessageStoreReads.ShouldBe<ulong>(0);
            result.Data[0].MessageStoreReadCountDetails.ShouldNotBeNull();
            result.Data[0].MessageStoreReadCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMessageStoreWrites.ShouldBe<ulong>(0);
            result.Data[0].MessageStoreReadCountDetails.ShouldNotBeNull();
            result.Data[0].MessageStoreReadCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMnesiaDiskTransactions.ShouldBe<ulong>(33);
            result.Data[0].MnesiaDiskTransactionCountDetails.ShouldNotBeNull();
            result.Data[0].MnesiaDiskTransactionCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMnesiaRamTransactions.ShouldBe<ulong>(1278);
            result.Data[0].MnesiaRAMTransactionCountDetails.ShouldNotBeNull();
            result.Data[0].MnesiaRAMTransactionCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueueIndexReads.ShouldBe<ulong>(5);
            result.Data[0].QueueIndexReadCountDetails.ShouldNotBeNull();
            result.Data[0].QueueIndexReadCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueueIndexWrites.ShouldBe<ulong>(43);
            result.Data[0].QueueIndexWriteCountDetails.ShouldNotBeNull();
            result.Data[0].QueueIndexWriteCountDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOBytesRead.ShouldBe<ulong>(44904556);
            result.Data[0].IOReadBytesDetails.ShouldNotBeNull();
            result.Data[0].IOReadBytesDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOBytesWritten.ShouldBe<ulong>(574531094);
            result.Data[0].IOWriteBytesDetail.ShouldNotBeNull();
            result.Data[0].IOWriteBytesDetail?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalOpenFileHandleAttempts.ShouldBe<ulong>(901142);
            result.Data[0].FileHandleOpenAttemptCountDetails.ShouldNotBeNull();
            result.Data[0].FileHandleOpenAttemptCountDetails?.Rate.ShouldBe(0.0M);
//            result.Data[0].OpenFileHandleAttemptsAvgTime.ShouldBe(0.0036650927378814886M);
            result.Data[0].FileHandleOpenAttemptAvgTimeDetails.ShouldNotBeNull();
            result.Data[0].FileHandleOpenAttemptAvgTimeDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalConnectionsCreated.ShouldBe<ulong>(15);
            result.Data[0].CreatedConnectionDetails.ShouldNotBeNull();
            result.Data[0].CreatedConnectionDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalConnectionsClosed.ShouldBe<ulong>(14);
            result.Data[0].ClosedConnectionDetails.ShouldNotBeNull();
            result.Data[0].ClosedConnectionDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalChannelsCreated.ShouldBe<ulong>(75);
            result.Data[0].CreatedChannelDetails.ShouldNotBeNull();
            result.Data[0].CreatedChannelDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalChannelsClosed.ShouldBe<ulong>(73);
            result.Data[0].ClosedChannelDetails.ShouldNotBeNull();
            result.Data[0].ClosedChannelDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesDeclared.ShouldBe<ulong>(11);
            result.Data[0].DeclaredQueueDetails.ShouldNotBeNull();
            result.Data[0].DeclaredQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesCreated.ShouldBe<ulong>(9);
            result.Data[0].CreatedQueueDetails.ShouldNotBeNull();
            result.Data[0].CreatedQueueDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesDeleted.ShouldBe<ulong>(7);
            result.Data[0].DeletedQueueDetails.ShouldNotBeNull();
            result.Data[0].DeletedQueueDetails?.Rate.ShouldBe(0.0M);
//            result.Data[0].AverageIOSeekTime.ShouldBe(0.8947999999999999M);
            result.Data[0].AvgIOSeekTimeDetails.ShouldNotBeNull();
            result.Data[0].AvgIOSeekTimeDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].IOSeekCount.ShouldBe<ulong>(80);
            result.Data[0].RateOfIOSeeks.ShouldNotBeNull();
            result.Data[0].RateOfIOSeeks?.Rate.ShouldBe(0.0M);
//            result.Data[0].AverageIOSyncTime.ShouldBe(8.185064377682403M);
            result.Data[0].AvgIOSyncTimeDetails.ShouldNotBeNull();
            result.Data[0].AvgIOSyncTimeDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].IOSyncCount.ShouldBe<ulong>(932);
            result.Data[0].RateOfIOSyncs.ShouldNotBeNull();
            result.Data[0].RateOfIOSyncs?.Rate.ShouldBe(0.0M);
            result.Data[0].BytesReclaimedByGarbageCollector.ShouldBe<ulong>(114844443912);
            result.Data[0].ReclaimedBytesFromGCDetails?.Rate.ShouldNotBeNull();
            result.Data[0].ReclaimedBytesFromGCDetails?.Rate.ShouldBe(147054.4M);
            result.Data[0].Type.ShouldBe("disc");
        }
    }
}