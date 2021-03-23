namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NewNodeTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_nodes1()
        {
            var services = GetContainerBuilder("TestData/NodeInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
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
                Assert.AreEqual(32.4M, result.Data[0].ContextSwitchDetails.Value);
                Assert.AreEqual("/usr/local/var/lib/rabbitmq/mnesia/rabbit@localhost", result.Data[0].DatabaseDirectory);
                Assert.AreEqual(5, result.Data[0].EnabledPlugins.Count);
                Assert.AreEqual("rabbitmq_management", result.Data[0].EnabledPlugins[0]);
                Assert.AreEqual(4, result.Data[0].ExchangeTypes.Count);
                Assert.AreEqual("headers", result.Data[0].ExchangeTypes[0].Name);
                Assert.AreEqual("AMQP headers exchange, as per the AMQP specification", result.Data[0].ExchangeTypes[0].Description);
                Assert.IsTrue(result.Data[0].ExchangeTypes[0].IsEnabled);
                Assert.IsNotNull(result.Data[0].GcDetails);
                Assert.AreEqual(6.0M, result.Data[0].GcDetails.Value);
                Assert.AreEqual(4815693, result.Data[0].NumberOfGarbageCollected);
                Assert.IsTrue(result.Data[0].IsRunning);
                Assert.IsFalse(result.Data[0].MemoryAlarm);
                Assert.AreEqual(429496729, result.Data[0].MemoryLimit);
                Assert.AreEqual(11784192, result.Data[0].MemoryUsed);
                Assert.IsNotNull(result.Data[0].MemoryUsageDetails);
                Assert.AreEqual(20480.0M, result.Data[0].MemoryUsageDetails.Value);
                Assert.AreEqual(1, result.Data[0].SocketsUsed);
                Assert.IsNotNull(result.Data[0].SocketsUsedDetails);
                Assert.AreEqual(0.0M, result.Data[0].SocketsUsedDetails.Value);
                Assert.AreEqual(138, result.Data[0].TotalSocketsAvailable);
                Assert.AreEqual(598, result.Data[0].ProcessesUsed);
                Assert.IsNotNull(result.Data[0].ProcessUsageDetails);
                Assert.AreEqual(0.0M, result.Data[0].ProcessUsageDetails?.Value);
                Assert.AreEqual(1048576, result.Data[0].TotalProcesses);
                Assert.AreEqual("1864", result.Data[0].OperatingSystemProcessId);
                Assert.AreEqual("basic", result.Data[0].RatesMode);
                Assert.AreEqual(4, result.Data[0].AvailableCoresDetected);
                Assert.IsFalse(result.Data[0].FreeDiskAlarm);
                Assert.AreEqual(50000000, result.Data[0].FreeDiskLimit);
                Assert.AreEqual(0, result.Data[0].FreeDiskSpace);
                Assert.IsNotNull(result.Data[0].FreeDiskSpaceDetails);
                Assert.AreEqual(0.0M, result.Data[0].FreeDiskSpaceDetails.Value);
                Assert.AreEqual(0, result.Data[0].FileDescriptorUsed);
                Assert.IsNotNull(result.Data[0].FileDescriptorUsedDetails);
                Assert.AreEqual(0.0M, result.Data[0].FileDescriptorUsedDetails.Value);
                Assert.AreEqual(256, result.Data[0].TotalFileDescriptors);
                Assert.AreEqual(60, result.Data[0].NetworkTickTime);
                Assert.AreEqual(11, result.Data[0].TotalIOReads);
                Assert.IsNotNull(result.Data[0].IOReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].IOReadDetails?.Value);
                Assert.AreEqual(932, result.Data[0].TotalIOWrites);
                Assert.IsNotNull(result.Data[0].IOWriteDetails);
                Assert.AreEqual(0.0M, result.Data[0].IOWriteDetails.Value);
                Assert.AreEqual(0, result.Data[0].TotalMessageStoreReads);
                Assert.IsNotNull(result.Data[0].MessageStoreReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].MessageStoreReadDetails.Value);
                Assert.AreEqual(0, result.Data[0].TotalMessageStoreWrites);
                Assert.IsNotNull(result.Data[0].MessageStoreReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].MessageStoreReadDetails.Value);
                Assert.AreEqual(33, result.Data[0].TotalMnesiaDiskTransactions);
                Assert.IsNotNull(result.Data[0].MnesiaDiskTransactionCountDetails);
                Assert.AreEqual(1278, result.Data[0].TotalMnesiaRamTransactions);
                Assert.AreEqual(0.0M, result.Data[0].MnesiaDiskTransactionCountDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].MnesiaRAMTransactionCountDetails.Value);
                Assert.IsNotNull(result.Data[0].MnesiaRAMTransactionCountDetails);
                Assert.AreEqual(5, result.Data[0].TotalQueueIndexReads);
                Assert.IsNotNull(result.Data[0].QueueIndexReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].QueueIndexReadDetails.Value);
                Assert.AreEqual(43, result.Data[0].TotalQueueIndexWrites);
                Assert.IsNotNull(result.Data[0].QueueIndexWriteDetails);
                Assert.AreEqual(0.0M, result.Data[0].QueueIndexWriteDetails.Value);
                Assert.AreEqual(44904556, result.Data[0].TotalIOBytesRead);
                Assert.AreEqual(0.0M, result.Data[0].IOBytesReadDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOBytesWrittenDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].FileHandleOpenAttemptDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].FileHandleOpenAttemptAvgTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedConnectionDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].ClosedConnectionDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedChannelDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].ClosedChannelDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].DeclaredQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].DeletedQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].AvgIOSeekTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOSeeksDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].AvgIOSyncTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOSyncsDetails?.Value);
                Assert.IsNotNull(result.Data[0].IOBytesReadDetails);
                Assert.IsNotNull(result.Data[0].IOBytesWrittenDetails);
                Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptDetails);
                Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptAvgTimeDetails);
                Assert.IsNotNull(result.Data[0].CreatedConnectionDetails);
                Assert.IsNotNull(result.Data[0].ClosedConnectionDetails);
                Assert.IsNotNull(result.Data[0].CreatedChannelDetails);
                Assert.IsNotNull(result.Data[0].ClosedChannelDetails);
                Assert.IsNotNull(result.Data[0].DeletedQueueDetails);
                Assert.IsNotNull(result.Data[0].DeclaredQueueDetails);
                Assert.IsNotNull(result.Data[0].CreatedQueueDetails);
                Assert.IsNotNull(result.Data[0].IOSyncsDetails);
                Assert.IsNotNull(result.Data[0].AvgIOSyncTimeDetails);
                Assert.IsNotNull(result.Data[0].IOSeeksDetails);
                Assert.IsNotNull(result.Data[0].AvgIOSeekTimeDetails);
                Assert.AreEqual(932, result.Data[0].IOSyncCount);
                Assert.AreEqual(574531094, result.Data[0].TotalIOBytesWritten);
                Assert.AreEqual(901142, result.Data[0].TotalOpenFileHandleAttempts);
                Assert.AreEqual(15, result.Data[0].TotalConnectionsCreated);
                Assert.AreEqual(14, result.Data[0].TotalConnectionsClosed);
                Assert.AreEqual(75, result.Data[0].TotalChannelsCreated);
                Assert.AreEqual(73, result.Data[0].TotalChannelsClosed);
                Assert.AreEqual(11, result.Data[0].TotalQueuesDeclared);
                Assert.AreEqual(9, result.Data[0].TotalQueuesCreated);
                Assert.AreEqual(7, result.Data[0].TotalQueuesDeleted);
                Assert.AreEqual(80, result.Data[0].IOSeekCount);
                Assert.AreEqual(114844443912, result.Data[0].BytesReclaimedByGarbageCollector);
                Assert.AreEqual(147054.4M, result.Data[0].ReclaimedBytesFromGCDetails.Value);
                Assert.AreEqual("disc", result.Data[0].Type);
            });
        }

        [Test]
        public async Task Should_be_able_to_get_all_nodes2()
        {
            var services = GetContainerBuilder("TestData/NodeInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllNodes();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
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
                Assert.AreEqual(32.4M, result.Data[0].ContextSwitchDetails.Value);
                Assert.AreEqual("/usr/local/var/lib/rabbitmq/mnesia/rabbit@localhost", result.Data[0].DatabaseDirectory);
                Assert.AreEqual(5, result.Data[0].EnabledPlugins.Count);
                Assert.AreEqual("rabbitmq_management", result.Data[0].EnabledPlugins[0]);
                Assert.AreEqual(4, result.Data[0].ExchangeTypes.Count);
                Assert.AreEqual("headers", result.Data[0].ExchangeTypes[0].Name);
                Assert.AreEqual("AMQP headers exchange, as per the AMQP specification", result.Data[0].ExchangeTypes[0].Description);
                Assert.IsTrue(result.Data[0].ExchangeTypes[0].IsEnabled);
                Assert.IsNotNull(result.Data[0].GcDetails);
                Assert.AreEqual(6.0M, result.Data[0].GcDetails.Value);
                Assert.AreEqual(4815693, result.Data[0].NumberOfGarbageCollected);
                Assert.IsTrue(result.Data[0].IsRunning);
                Assert.IsFalse(result.Data[0].MemoryAlarm);
                Assert.AreEqual(429496729, result.Data[0].MemoryLimit);
                Assert.AreEqual(11784192, result.Data[0].MemoryUsed);
                Assert.IsNotNull(result.Data[0].MemoryUsageDetails);
                Assert.AreEqual(20480.0M, result.Data[0].MemoryUsageDetails.Value);
                Assert.AreEqual(1, result.Data[0].SocketsUsed);
                Assert.IsNotNull(result.Data[0].SocketsUsedDetails);
                Assert.AreEqual(0.0M, result.Data[0].SocketsUsedDetails.Value);
                Assert.AreEqual(138, result.Data[0].TotalSocketsAvailable);
                Assert.AreEqual(598, result.Data[0].ProcessesUsed);
                Assert.IsNotNull(result.Data[0].ProcessUsageDetails);
                Assert.AreEqual(0.0M, result.Data[0].ProcessUsageDetails?.Value);
                Assert.AreEqual(1048576, result.Data[0].TotalProcesses);
                Assert.AreEqual("1864", result.Data[0].OperatingSystemProcessId);
                Assert.AreEqual("basic", result.Data[0].RatesMode);
                Assert.AreEqual(4, result.Data[0].AvailableCoresDetected);
                Assert.IsFalse(result.Data[0].FreeDiskAlarm);
                Assert.AreEqual(50000000, result.Data[0].FreeDiskLimit);
                Assert.AreEqual(0, result.Data[0].FreeDiskSpace);
                Assert.IsNotNull(result.Data[0].FreeDiskSpaceDetails);
                Assert.AreEqual(0.0M, result.Data[0].FreeDiskSpaceDetails.Value);
                Assert.AreEqual(0, result.Data[0].FileDescriptorUsed);
                Assert.IsNotNull(result.Data[0].FileDescriptorUsedDetails);
                Assert.AreEqual(0.0M, result.Data[0].FileDescriptorUsedDetails.Value);
                Assert.AreEqual(256, result.Data[0].TotalFileDescriptors);
                Assert.AreEqual(60, result.Data[0].NetworkTickTime);
                Assert.AreEqual(11, result.Data[0].TotalIOReads);
                Assert.IsNotNull(result.Data[0].IOReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].IOReadDetails?.Value);
                Assert.AreEqual(932, result.Data[0].TotalIOWrites);
                Assert.IsNotNull(result.Data[0].IOWriteDetails);
                Assert.AreEqual(0.0M, result.Data[0].IOWriteDetails.Value);
                Assert.AreEqual(0, result.Data[0].TotalMessageStoreReads);
                Assert.IsNotNull(result.Data[0].MessageStoreReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].MessageStoreReadDetails.Value);
                Assert.AreEqual(0, result.Data[0].TotalMessageStoreWrites);
                Assert.IsNotNull(result.Data[0].MessageStoreReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].MessageStoreReadDetails.Value);
                Assert.AreEqual(33, result.Data[0].TotalMnesiaDiskTransactions);
                Assert.IsNotNull(result.Data[0].MnesiaDiskTransactionCountDetails);
                Assert.AreEqual(1278, result.Data[0].TotalMnesiaRamTransactions);
                Assert.AreEqual(0.0M, result.Data[0].MnesiaDiskTransactionCountDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].MnesiaRAMTransactionCountDetails.Value);
                Assert.IsNotNull(result.Data[0].MnesiaRAMTransactionCountDetails);
                Assert.AreEqual(5, result.Data[0].TotalQueueIndexReads);
                Assert.IsNotNull(result.Data[0].QueueIndexReadDetails);
                Assert.AreEqual(0.0M, result.Data[0].QueueIndexReadDetails.Value);
                Assert.AreEqual(43, result.Data[0].TotalQueueIndexWrites);
                Assert.IsNotNull(result.Data[0].QueueIndexWriteDetails);
                Assert.AreEqual(0.0M, result.Data[0].QueueIndexWriteDetails.Value);
                Assert.AreEqual(44904556, result.Data[0].TotalIOBytesRead);
                Assert.AreEqual(0.0M, result.Data[0].IOBytesReadDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOBytesWrittenDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].FileHandleOpenAttemptDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].FileHandleOpenAttemptAvgTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedConnectionDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].ClosedConnectionDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedChannelDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].ClosedChannelDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].DeclaredQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].CreatedQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].DeletedQueueDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].AvgIOSeekTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOSeeksDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].AvgIOSyncTimeDetails.Value);
                Assert.AreEqual(0.0M, result.Data[0].IOSyncsDetails?.Value);
                Assert.IsNotNull(result.Data[0].IOBytesReadDetails);
                Assert.IsNotNull(result.Data[0].IOBytesWrittenDetails);
                Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptDetails);
                Assert.IsNotNull(result.Data[0].FileHandleOpenAttemptAvgTimeDetails);
                Assert.IsNotNull(result.Data[0].CreatedConnectionDetails);
                Assert.IsNotNull(result.Data[0].ClosedConnectionDetails);
                Assert.IsNotNull(result.Data[0].CreatedChannelDetails);
                Assert.IsNotNull(result.Data[0].ClosedChannelDetails);
                Assert.IsNotNull(result.Data[0].DeletedQueueDetails);
                Assert.IsNotNull(result.Data[0].DeclaredQueueDetails);
                Assert.IsNotNull(result.Data[0].CreatedQueueDetails);
                Assert.IsNotNull(result.Data[0].IOSyncsDetails);
                Assert.IsNotNull(result.Data[0].AvgIOSyncTimeDetails);
                Assert.IsNotNull(result.Data[0].IOSeeksDetails);
                Assert.IsNotNull(result.Data[0].AvgIOSeekTimeDetails);
                Assert.AreEqual(932, result.Data[0].IOSyncCount);
                Assert.AreEqual(574531094, result.Data[0].TotalIOBytesWritten);
                Assert.AreEqual(901142, result.Data[0].TotalOpenFileHandleAttempts);
                Assert.AreEqual(15, result.Data[0].TotalConnectionsCreated);
                Assert.AreEqual(14, result.Data[0].TotalConnectionsClosed);
                Assert.AreEqual(75, result.Data[0].TotalChannelsCreated);
                Assert.AreEqual(73, result.Data[0].TotalChannelsClosed);
                Assert.AreEqual(11, result.Data[0].TotalQueuesDeclared);
                Assert.AreEqual(9, result.Data[0].TotalQueuesCreated);
                Assert.AreEqual(7, result.Data[0].TotalQueuesDeleted);
                Assert.AreEqual(80, result.Data[0].IOSeekCount);
                Assert.AreEqual(114844443912, result.Data[0].BytesReclaimedByGarbageCollector);
                Assert.AreEqual(147054.4M, result.Data[0].ReclaimedBytesFromGCDetails.Value);
                Assert.AreEqual("disc", result.Data[0].Type);
            });
        }
        
        [Test]
        public async Task Verify_will_return_node_memory_usage1()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage("haredu@localhost");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data.Memory);
                Assert.AreEqual(0, result.Data.Memory.ConnectionReaders);
                Assert.AreEqual(0, result.Data.Memory.ConnectionWriters);
                Assert.AreEqual(0, result.Data.Memory.ConnectionChannels);
                Assert.AreEqual(0, result.Data.Memory.QueueSlaveProcesses);
                Assert.AreEqual(0, result.Data.Memory.ReservedUnallocated);
                Assert.AreEqual(132692, result.Data.Memory.ConnectionOther);
                Assert.AreEqual(210444, result.Data.Memory.QueueProcesses);
                Assert.AreEqual(76132, result.Data.Memory.QuorumQueueProcesses);
                Assert.AreEqual(4035284, result.Data.Memory.Plugins);
                Assert.AreEqual(23706508, result.Data.Memory.OtherProcesses);
                Assert.AreEqual(235692, result.Data.Memory.Metrics);
                Assert.AreEqual(1053904, result.Data.Memory.ManagementDatabase);
                Assert.AreEqual(190488, result.Data.Memory.Mnesia);
                Assert.AreEqual(45464, result.Data.Memory.QuorumInMemoryStorage);
                Assert.AreEqual(3508368, result.Data.Memory.OtherInMemoryStorage);
                Assert.AreEqual(771152, result.Data.Memory.Binary);
                Assert.AreEqual(338800, result.Data.Memory.MessageIndex);
                Assert.AreEqual(27056269, result.Data.Memory.ByteCode);
                Assert.AreEqual(1566897, result.Data.Memory.Atom);
                Assert.AreEqual(16660090, result.Data.Memory.OtherSystem);
                Assert.AreEqual(17765544, result.Data.Memory.AllocatedUnused);
                Assert.AreEqual("rss", result.Data.Memory.Strategy);
                Assert.AreEqual(79588184, result.Data.Memory.Total.Erlang);
                Assert.AreEqual(32055296, result.Data.Memory.Total.Strategy);
                Assert.AreEqual(97353728, result.Data.Memory.Total.Allocated);
            });
        }
        
        [Test]
        public async Task Verify_will_return_node_memory_usage2()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeMemoryUsage("haredu@localhost");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data.Memory);
                Assert.AreEqual(0, result.Data.Memory.ConnectionReaders);
                Assert.AreEqual(0, result.Data.Memory.ConnectionWriters);
                Assert.AreEqual(0, result.Data.Memory.ConnectionChannels);
                Assert.AreEqual(0, result.Data.Memory.QueueSlaveProcesses);
                Assert.AreEqual(0, result.Data.Memory.ReservedUnallocated);
                Assert.AreEqual(132692, result.Data.Memory.ConnectionOther);
                Assert.AreEqual(210444, result.Data.Memory.QueueProcesses);
                Assert.AreEqual(76132, result.Data.Memory.QuorumQueueProcesses);
                Assert.AreEqual(4035284, result.Data.Memory.Plugins);
                Assert.AreEqual(23706508, result.Data.Memory.OtherProcesses);
                Assert.AreEqual(235692, result.Data.Memory.Metrics);
                Assert.AreEqual(1053904, result.Data.Memory.ManagementDatabase);
                Assert.AreEqual(190488, result.Data.Memory.Mnesia);
                Assert.AreEqual(45464, result.Data.Memory.QuorumInMemoryStorage);
                Assert.AreEqual(3508368, result.Data.Memory.OtherInMemoryStorage);
                Assert.AreEqual(771152, result.Data.Memory.Binary);
                Assert.AreEqual(338800, result.Data.Memory.MessageIndex);
                Assert.AreEqual(27056269, result.Data.Memory.ByteCode);
                Assert.AreEqual(1566897, result.Data.Memory.Atom);
                Assert.AreEqual(16660090, result.Data.Memory.OtherSystem);
                Assert.AreEqual(17765544, result.Data.Memory.AllocatedUnused);
                Assert.AreEqual("rss", result.Data.Memory.Strategy);
                Assert.AreEqual(79588184, result.Data.Memory.Total.Erlang);
                Assert.AreEqual(32055296, result.Data.Memory.Total.Strategy);
                Assert.AreEqual(97353728, result.Data.Memory.Total.Allocated);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing1()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage(string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing2()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeMemoryUsage(string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing3()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage(null);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing4()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeMemoryUsage(null);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing5()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage("   ");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }

        [Test]
        public async Task Verify_will_not_return_node_memory_usage_when_node_missing6()
        {
            var services = GetContainerBuilder("TestData/MemoryUsageInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeMemoryUsage("   ");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsFalse(result.HasData);
            });
        }
        
        [Test]
        public async Task Verify_can_check_if_named_node_healthy1()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Ok.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth("rabbit@localhost");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Ok, result.Data.Status);
            });
        }
        
        [Test]
        public async Task Verify_can_check_if_named_node_healthy2()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Ok.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeHealth("rabbit@localhost");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Ok, result.Data.Status);
            });
        }
        
        [Test]
        public async Task Verify_can_check_if_named_node_unhealthy1()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Failed.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth("rabbit@localhost");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Failed, result.Data.Status);
            });
        }
        
        [Test]
        public async Task Verify_can_check_if_named_node_unhealthy2()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Failed.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeHealth("rabbit@localhost");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Failed, result.Data.Status);
            });
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy1()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Ok.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Ok, result.Data.Status);
            });
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy2()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Ok.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeHealth();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Ok, result.Data.Status);
            });
        }

        [Test]
        public async Task Verify_can_check_if_node_unhealthy1()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Failed.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Failed, result.Data.Status);
            });
        }

        [Test]
        public async Task Verify_can_check_if_node_unhealthy2()
        {
            var services = GetContainerBuilder("TestData/NodeHealthInfo_Failed.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetNodeHealth();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(NodeStatus.Failed, result.Data.Status);
            });
        }
    }
}