namespace HareDu.Tests
{
    using Core.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class NodeTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_nodes()
        {
            var container = GetContainerBuilder("TestData/NodeInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll()
                .GetResult();

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
            result.Data[0].ContextSwitchDetails.Rate.ShouldBe(32.4M);
            result.Data[0].DatabaseDirectory.ShouldBe("/usr/local/var/lib/rabbitmq/mnesia/rabbit@localhost");
            result.Data[0].EnabledPlugins.Count.ShouldBe(5);
            result.Data[0].EnabledPlugins[0].ShouldBe("rabbitmq_management");
            result.Data[0].ExchangeTypes.Count.ShouldBe(4);
            result.Data[0].ExchangeTypes[0].Name.ShouldBe("headers");
            result.Data[0].ExchangeTypes[0].Description.ShouldBe("AMQP headers exchange, as per the AMQP specification");
            result.Data[0].ExchangeTypes[0].IsEnabled.ShouldBeTrue();
            result.Data[0].GcDetails.ShouldNotBeNull();
            result.Data[0].GcDetails.Rate.ShouldBe(6.0M);
            result.Data[0].NumberOfGarbageCollected.ShouldBe<ulong>(4815693);
            result.Data[0].IsRunning.ShouldBeTrue();
            result.Data[0].MemoryAlarm.ShouldBeFalse();
            result.Data[0].MemoryLimit.ShouldBe<ulong>(429496729);
            result.Data[0].MemoryUsed.ShouldBe<ulong>(11784192);
            result.Data[0].MemoryUsageDetails.ShouldNotBeNull();
            result.Data[0].MemoryUsageDetails.Rate.ShouldBe(20480.0M);
            result.Data[0].SocketsUsed.ShouldBe<ulong>(1);
            result.Data[0].SocketsUsedDetails.ShouldNotBeNull();
            result.Data[0].SocketsUsedDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalSocketsAvailable.ShouldBe<ulong>(138);
            result.Data[0].ProcessesUsed.ShouldBe<ulong>(598);
            result.Data[0].ProcessUsageDetails.ShouldNotBeNull();
            result.Data[0].ProcessUsageDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalProcesses.ShouldBe<ulong>(1048576);
            result.Data[0].OperatingSystemProcessId.ShouldBe("1864");
            result.Data[0].RatesMode.ShouldBe("basic");
            result.Data[0].AvailableCoresDetected.ShouldBe<uint>(4);
            result.Data[0].FreeDiskAlarm.ShouldBeFalse();
            result.Data[0].FreeDiskLimit.ShouldBe<ulong>(50000000);
            result.Data[0].FreeDiskSpace.ShouldBe<ulong>(0);
            result.Data[0].FreeDiskSpaceDetails.ShouldNotBeNull();
            result.Data[0].FreeDiskSpaceDetails.Rate.ShouldBe(0.0M);
            result.Data[0].FileDescriptorUsed.ShouldBe<ulong>(0);
            result.Data[0].FileDescriptorUsedDetails.ShouldNotBeNull();
            result.Data[0].FileDescriptorUsedDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalFileDescriptors.ShouldBe<ulong>(256);
            result.Data[0].NetworkTickTime.ShouldBe(60);
            result.Data[0].TotalIOReads.ShouldBe<ulong>(11);
            result.Data[0].IOReadDetails.ShouldNotBeNull();
            result.Data[0].IOReadDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOWrites.ShouldBe<ulong>(932);
            result.Data[0].IOWriteDetails.ShouldNotBeNull();
            result.Data[0].IOWriteDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMessageStoreReads.ShouldBe<ulong>(0);
            result.Data[0].MessageStoreReadDetails.ShouldNotBeNull();
            result.Data[0].MessageStoreReadDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMessageStoreWrites.ShouldBe<ulong>(0);
            result.Data[0].MessageStoreReadDetails.ShouldNotBeNull();
            result.Data[0].MessageStoreReadDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMnesiaDiskTransactions.ShouldBe<ulong>(33);
            result.Data[0].MnesiaDiskTransactionCountDetails.ShouldNotBeNull();
            result.Data[0].MnesiaDiskTransactionCountDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalMnesiaRamTransactions.ShouldBe<ulong>(1278);
            result.Data[0].MnesiaRAMTransactionCountDetails.ShouldNotBeNull();
            result.Data[0].MnesiaRAMTransactionCountDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueueIndexReads.ShouldBe<ulong>(5);
            result.Data[0].QueueIndexReadDetails.ShouldNotBeNull();
            result.Data[0].QueueIndexReadDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueueIndexWrites.ShouldBe<ulong>(43);
            result.Data[0].QueueIndexWriteDetails.ShouldNotBeNull();
            result.Data[0].QueueIndexWriteDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOBytesRead.ShouldBe<ulong>(44904556);
            result.Data[0].IOBytesReadDetails.ShouldNotBeNull();
            result.Data[0].IOBytesReadDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalIOBytesWritten.ShouldBe<ulong>(574531094);
            result.Data[0].IOBytesWrittenDetails.ShouldNotBeNull();
            result.Data[0].IOBytesWrittenDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalOpenFileHandleAttempts.ShouldBe<ulong>(901142);
            result.Data[0].FileHandleOpenAttemptDetails.ShouldNotBeNull();
            result.Data[0].FileHandleOpenAttemptDetails.Rate.ShouldBe(0.0M);
//            result.Data[0].OpenFileHandleAttemptsAvgTime.ShouldBe(0.0036650927378814886M);
            result.Data[0].FileHandleOpenAttemptAvgTimeDetails.ShouldNotBeNull();
            result.Data[0].FileHandleOpenAttemptAvgTimeDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalConnectionsCreated.ShouldBe<ulong>(15);
            result.Data[0].CreatedConnectionDetails.ShouldNotBeNull();
            result.Data[0].CreatedConnectionDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalConnectionsClosed.ShouldBe<ulong>(14);
            result.Data[0].ClosedConnectionDetails.ShouldNotBeNull();
            result.Data[0].ClosedConnectionDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalChannelsCreated.ShouldBe<ulong>(75);
            result.Data[0].CreatedChannelDetails.ShouldNotBeNull();
            result.Data[0].CreatedChannelDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalChannelsClosed.ShouldBe<ulong>(73);
            result.Data[0].ClosedChannelDetails.ShouldNotBeNull();
            result.Data[0].ClosedChannelDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesDeclared.ShouldBe<ulong>(11);
            result.Data[0].DeclaredQueueDetails.ShouldNotBeNull();
            result.Data[0].DeclaredQueueDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesCreated.ShouldBe<ulong>(9);
            result.Data[0].CreatedQueueDetails.ShouldNotBeNull();
            result.Data[0].CreatedQueueDetails.Rate.ShouldBe(0.0M);
            result.Data[0].TotalQueuesDeleted.ShouldBe<ulong>(7);
            result.Data[0].DeletedQueueDetails.ShouldNotBeNull();
            result.Data[0].DeletedQueueDetails.Rate.ShouldBe(0.0M);
//            result.Data[0].AverageIOSeekTime.ShouldBe(0.8947999999999999M);
            result.Data[0].AvgIOSeekTimeDetails.ShouldNotBeNull();
            result.Data[0].AvgIOSeekTimeDetails.Rate.ShouldBe(0.0M);
            result.Data[0].IOSeekCount.ShouldBe<ulong>(80);
            result.Data[0].IOSeeksDetails.ShouldNotBeNull();
            result.Data[0].IOSeeksDetails.Rate.ShouldBe(0.0M);
//            result.Data[0].AverageIOSyncTime.ShouldBe(8.185064377682403M);
            result.Data[0].AvgIOSyncTimeDetails.ShouldNotBeNull();
            result.Data[0].AvgIOSyncTimeDetails.Rate.ShouldBe(0.0M);
            result.Data[0].IOSyncCount.ShouldBe<ulong>(932);
            result.Data[0].IOSyncsDetails.ShouldNotBeNull();
            result.Data[0].IOSyncsDetails?.Rate.ShouldBe(0.0M);
            result.Data[0].BytesReclaimedByGarbageCollector.ShouldBe<ulong>(114844443912);
            result.Data[0].ReclaimedBytesFromGCDetails.Rate.ShouldNotBeNull();
            result.Data[0].ReclaimedBytesFromGCDetails.Rate.ShouldBe(147054.4M);
            result.Data[0].Type.ShouldBe("disc");
        }
        
        [Test]
        public void Verify_will_return_node_memory_usage()
        {
            var container = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage("haredu@localhost")
                .GetResult();
            
            result.HasData.ShouldBeTrue();
            result.HasFaulted.ShouldBeFalse();
            result.Data.Memory.ShouldNotBeNull();
            result.Data.Memory.ConnectionReaders.ShouldBe(0);
            result.Data.Memory.ConnectionWriters.ShouldBe(0);
            result.Data.Memory.ConnectionChannels.ShouldBe(0);
            result.Data.Memory.ConnectionOther.ShouldBe(132692);
            result.Data.Memory.QueueProcesses.ShouldBe(210444);
            result.Data.Memory.QueueSlaveProcesses.ShouldBe(0);
            result.Data.Memory.QuorumQueueProcesses.ShouldBe(76132);
            result.Data.Memory.Plugins.ShouldBe(4035284);
            result.Data.Memory.OtherProcesses.ShouldBe(23706508);
            result.Data.Memory.Metrics.ShouldBe(235692);
            result.Data.Memory.ManagementDatabase.ShouldBe(1053904);
            result.Data.Memory.Mnesia.ShouldBe(190488);
            result.Data.Memory.QuorumInMemoryStorage.ShouldBe(45464);
            result.Data.Memory.OtherInMemoryStorage.ShouldBe(3508368);
            result.Data.Memory.Binary.ShouldBe(771152);
            result.Data.Memory.MessageIndex.ShouldBe(338800);
            result.Data.Memory.ByteCode.ShouldBe(27056269);
            result.Data.Memory.Atom.ShouldBe(1566897);
            result.Data.Memory.OtherSystem.ShouldBe(16660090);
            result.Data.Memory.AllocatedUnused.ShouldBe(17765544);
            result.Data.Memory.ReservedUnallocated.ShouldBe(0);
            result.Data.Memory.Strategy.ShouldBe("rss");
            result.Data.Memory.Total.Erlang.ShouldBe(79588184);
            result.Data.Memory.Total.Strategy.ShouldBe(32055296);
            result.Data.Memory.Total.Allocated.ShouldBe(97353728);
        }

        [Test]
        public void Verify_will_not_return_node_memory_usage_when_node_missing_1()
        {
            var container = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage(string.Empty)
                .GetResult();
            
            result.HasData.ShouldBeFalse();
            result.HasFaulted.ShouldBeTrue();
        }

        [Test]
        public void Verify_will_not_return_node_memory_usage_when_node_missing_2()
        {
            var container = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage(null)
                .GetResult();
            
            result.HasData.ShouldBeFalse();
            result.HasFaulted.ShouldBeTrue();
        }

        [Test]
        public void Verify_will_not_return_node_memory_usage_when_node_missing_3()
        {
            var container = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage("   ")
                .GetResult();
            
            result.HasData.ShouldBeFalse();
            result.HasFaulted.ShouldBeTrue();
        }
        
        [Test]
        public void Verify_can_check_if_named_node_healthy()
        {
            var container = GetContainerBuilder("TestData/NodeHealthInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth("rabbit@localhost")
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.Status.ShouldBe("ok");
        }

        [Test]
        public void Verify_can_check_if_node_healthy()
        {
            var container = GetContainerBuilder("TestData/NodeHealthInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth()
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.Status.ShouldBe("ok");
        }
    }
}