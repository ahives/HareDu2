namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Test()
        {
            Result<IReadOnlyList<ChannelInfo>> result = await Client
                .Factory<Node>()
                .GetChannels();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", node.Name);
                Console.WriteLine("VirtualHost: {0}", node.VirtualHost);
                Console.WriteLine("Host: {0}", node.Host);
                Console.WriteLine("TotalChannels: {0}", node.TotalChannels);
                Console.WriteLine("TotalConsumers: {0}", node.TotalConsumers);
//                Console.WriteLine("Host: {0}", node);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }
        
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_nodes()
        {
            Result<IReadOnlyList<NodeInfo>> result = await Client
                .Factory<Cluster>()
                .GetNodes();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemPid);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedRate.Rate);
                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedRate.Rate);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_connections()
        {
            Result<IReadOnlyList<ConnectionInfo>> result = await Client
                .Factory<Node>()
                .GetConnections();

            foreach (var connection in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", connection.Name);
                Console.WriteLine("Channels: {0}", connection.Channels);
                Console.WriteLine("AuthenticationMechanism: {0}", connection.AuthenticationMechanism);
                Console.WriteLine("ConnectedAt: {0}", connection.ConnectedAt);
                Console.WriteLine("ConnectionTimeout: {0}", connection.ConnectionTimeout);
                Console.WriteLine("FullSweepAfter: {0}", connection.GarbageCollectionDetails.FullSweepAfter);
                Console.WriteLine("MaximumHeapSize: {0}", connection.GarbageCollectionDetails.MaximumHeapSize);
                Console.WriteLine("MinimumBinaryVirtualHeapSize: {0}", connection.GarbageCollectionDetails.MinimumBinaryVirtualHeapSize);
                Console.WriteLine("MinimumHeapSize: {0}", connection.GarbageCollectionDetails.MinimumHeapSize);
                Console.WriteLine("MinorGarbageCollection: {0}", connection.GarbageCollectionDetails.MinorGarbageCollection);
                Console.WriteLine("Host: {0}", connection.Host);
                Console.WriteLine("IsSecure: {0}", connection.IsSecure);
                Console.WriteLine("MaxChannels: {0}", connection.MaxChannels);
                Console.WriteLine("MaxFrameSizeInBytes: {0}", connection.MaxFrameSizeInBytes);
                Console.WriteLine("OctetsReceived: {0}", connection.OctetsReceived);
                Console.WriteLine("PacketsReceived: {0}", connection.PacketsReceived);
                Console.WriteLine("PeerCertificateIssuer: {0}", connection.PeerCertificateIssuer);
                Console.WriteLine("PeerCertificateSubject: {0}", connection.PeerCertificateSubject);
                Console.WriteLine("PeerHost: {0}", connection.PeerHost);
                Console.WriteLine("PeerPort: {0}", connection.PeerPort);
                Console.WriteLine("Port: {0}", connection.Port);
                Console.WriteLine("RateOfOctetsRecevied: {0}", connection.RateOfOctetsRecevied.Rate);
                Console.WriteLine("RateOfOctetsSent: {0}", connection.RateOfOctetsSent.Rate);
                Console.WriteLine("RateOfReduction: {0}", connection.RateOfReduction.Rate);
                Console.WriteLine("SendPending: {0}", connection.SendPending);
                Console.WriteLine("SslCipherAlgorithm: {0}", connection.SslCipherAlgorithm);
                Console.WriteLine("SslHashFunction: {0}", connection.SslHashFunction);
                Console.WriteLine("SslKeyExchangeAlgorithm: {0}", connection.SslKeyExchangeAlgorithm);
                Console.WriteLine("SslProtocol: {0}", connection.SslProtocol);
                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("TimePeriodPeerCertificateValid: {0}", connection.TimePeriodPeerCertificateValid);
                Console.WriteLine("TotalReductions: {0}", connection.TotalReductions);
                Console.WriteLine("Type: {0}", connection.Type);
                Console.WriteLine("VirtualHost: {0}", connection.VirtualHost);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_channels()
        {
            Result<IReadOnlyList<ChannelInfo>> result = await Client
                .Factory<Node>()
                .GetChannels();

            if (result.HasResult)
            {
                foreach (var node in result.Select(x => x.Data))
                {
                    Console.WriteLine("Name: {0}", node.Name);
                    Console.WriteLine("PrefetchCount: {0}", node.PrefetchCount);
                    Console.WriteLine("AuthenticationMechanism: {0}", node.AuthenticationMechanism);
                    Console.WriteLine("ChannelMax: {0}", node.ChannelMax);
                    Console.WriteLine("Confirm: {0}", node.Confirm);
                    Console.WriteLine("ConnectedAt: {0}", node.ConnectedAt);
                    Console.WriteLine("ConnectionDetailsName: {0}", node.ConnectionDetails.Name);
                    Console.WriteLine("PeerHost: {0}", node.ConnectionDetails.PeerHost);
                    Console.WriteLine("PeerPort: {0}", node.ConnectionDetails.PeerPort);
                    Console.WriteLine("FrameMax: {0}", node.FrameMax);
                    Console.WriteLine("FullSweepAfter: {0}", node.GarbageCollectionDetails.FullSweepAfter);
                    Console.WriteLine("MaximumHeapSize: {0}", node.GarbageCollectionDetails.MaximumHeapSize);
                    Console.WriteLine("MinimumBinaryVirtualHeapSize: {0}",
                        node.GarbageCollectionDetails.MinimumBinaryVirtualHeapSize);
                    Console.WriteLine("MinimumHeapSize: {0}", node.GarbageCollectionDetails.MinimumHeapSize);
                    Console.WriteLine("MinorGarbageCollection: {0}",
                        node.GarbageCollectionDetails.MinorGarbageCollection);
                    Console.WriteLine("GlobalPrefetchCount: {0}", node.GlobalPrefetchCount);
                    Console.WriteLine("Host: {0}", node.Host);
                    Console.WriteLine("IdleSince: {0}", node.IdleSince.ToString());
                    Console.WriteLine("Node: {0}", node.Node);
                    Console.WriteLine("Number: {0}", node.Number);
                    Console.WriteLine("PeerCertificateIssuer: {0}", node.PeerCertificateIssuer);
                    Console.WriteLine("PeerCertificateSubject: {0}", node.PeerCertificateSubject);
                    Console.WriteLine("PeerCertificateValidity: {0}", node.PeerCertificateValidity);
                    Console.WriteLine("PeerHost: {0}", node.PeerHost);
                    Console.WriteLine("PeerPort: {0}", node.PeerPort);
                    Console.WriteLine("Port: {0}", node.Port);
                    Console.WriteLine("PrefetchCount: {0}", node.PrefetchCount);
                    Console.WriteLine("Protocol: {0}", node.Protocol);
                    Console.WriteLine("RateOfReceviedOctet: {0}", node.RateOfReceviedOctet.Rate);
                    Console.WriteLine("RateOfReduction: {0}", node.RateOfReduction.Rate);
                    Console.WriteLine("RateOfSentOctet: {0}", node.RateOfSentOctet.Rate);
                    Console.WriteLine("ReceivedOctet: {0}", node.ReceivedOctet);
                    Console.WriteLine("SendOctet: {0}", node.SendOctet);
                    Console.WriteLine("SentPending: {0}", node.SentPending);
                    Console.WriteLine("Ssl: {0}", node.Ssl);
                    Console.WriteLine("SslCipher: {0}", node.SslCipher);
                    Console.WriteLine("SslHash: {0}", node.SslHash);
                    Console.WriteLine("SslKeyExchange: {0}", node.SslKeyExchange);
                    Console.WriteLine("SslProtocol: {0}", node.SslProtocol);
                    Console.WriteLine("State: {0}", node.State);
                    Console.WriteLine("Timeout: {0}", node.Timeout);
                    Console.WriteLine("TotalChannels: {0}", node.TotalChannels);
                    Console.WriteLine("TotalConsumers: {0}", node.TotalConsumers);
                    Console.WriteLine("TotalReceived: {0}", node.TotalReceived);
                    Console.WriteLine("TotalReductions: {0}", node.TotalReductions);
                    Console.WriteLine("TotalSent: {0}", node.TotalSent);
                    Console.WriteLine("Transactional: {0}", node.Transactional);
                    Console.WriteLine("Type: {0}", node.Type);
                    Console.WriteLine("UnacknowledgedMessages: {0}", node.UnacknowledgedMessages);
                    Console.WriteLine("UncommittedAcknowledgements: {0}", node.UncommittedAcknowledgements);
                    Console.WriteLine("UncommittedMessages: {0}", node.UncommittedMessages);
                    Console.WriteLine("UnconfirmedMessages: {0}", node.UnconfirmedMessages);
                    Console.WriteLine("User: {0}", node.User);
                    Console.WriteLine("VirtualHost: {0}", node.VirtualHost);
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_consumers()
        {
            Result<IReadOnlyList<ConsumerInfo>> result = await Client
                .Factory<Node>()
                .GetConsumers();

            if (result.HasResult)
            {
                foreach (var consumer in result.Select(x => x.Data))
                {
                    Console.WriteLine("AcknowledgementRequired: {0}", consumer.AcknowledgementRequired);
                    Console.WriteLine("Arguments: {0}", consumer.Arguments);
                    Console.WriteLine("Name: {0}", consumer.ChannelDetails?.Name);
                    Console.WriteLine("ConnectionName: {0}", consumer.ChannelDetails?.ConnectionName);
                    Console.WriteLine("Node: {0}", consumer.ChannelDetails?.Node);
                    Console.WriteLine("Number: {0}", consumer.ChannelDetails?.Number);
                    Console.WriteLine("PeerHost: {0}", consumer.ChannelDetails?.PeerHost);
                    Console.WriteLine("PeerPort: {0}", consumer.ChannelDetails?.PeerPort);
                    Console.WriteLine("User: {0}", consumer.ChannelDetails?.User);
                    Console.WriteLine("ConsumerTag: {0}", consumer.ConsumerTag);
                    Console.WriteLine("Exclusive: {0}", consumer.Exclusive);
                    Console.WriteLine("PreFetchCount: {0}", consumer.PreFetchCount);
                    Console.WriteLine("Name: {0}", consumer.QueueConsumerDetails?.Name);
                    Console.WriteLine("VirtualHost: {0}", consumer.QueueConsumerDetails?.VirtualHost);
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
        }

        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            Result<ServerDefinitionInfo> result = await Client
                .Factory<Node>()
                .GetDefinition();

            if (result.HasResult)
            {
                var definition = result.Data;
                
                Console.WriteLine("RabbitMqVersion: {0}", definition.RabbitMqVersion);
                
                foreach (var binding in definition.Bindings)
                {
                    Console.WriteLine("Destination: {0}", binding.Destination);
                    Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                    Console.WriteLine("PropertiesKey: {0}", binding.PropertiesKey);
                    Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                    Console.WriteLine("Source: {0}", binding.Source);
                    Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                }
                
                foreach (var exchange in definition.Exchanges)
                {
                    Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                    Console.WriteLine("Durable: {0}", exchange.Durable);
                    Console.WriteLine("Internal: {0}", exchange.Internal);
                    Console.WriteLine("Name: {0}", exchange.Name);
                    Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                    Console.WriteLine("VirtualHost: {0}", exchange.VirtualHost);
                }
                
                foreach (var queue in definition.Queues)
                {
                    Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                    Console.WriteLine("Durable: {0}", queue.Durable);
                    Console.WriteLine("Internal: {0}", queue.Consumers);
                    Console.WriteLine("Name: {0}", queue.Name);
                    Console.WriteLine("ConsumerUtilization: {0}", queue.ConsumerUtilization);
                    Console.WriteLine("Exclusive: {0}", queue.Exclusive);
                    Console.WriteLine("IdleSince: {0}", queue.IdleSince.ToString());
                    Console.WriteLine("HeadMessageTimestamp: {0}", queue.HeadMessageTimestamp.ToString());
                    Console.WriteLine("MinorGarbageCollection: {0}", queue.GC?.MinorGarbageCollection);
                    Console.WriteLine("MinimumHeapSize: {0}", queue.GC?.MinimumHeapSize);
                    Console.WriteLine("MinimumBinaryVirtualHeapSize: {0}", queue.GC?.MinimumBinaryVirtualHeapSize);
                    Console.WriteLine("MaximumHeapSize: {0}", queue.GC?.MaximumHeapSize);
                    Console.WriteLine("FullSweepAfter: {0}", queue.GC?.FullSweepAfter);
                    Console.WriteLine("ExclusiveConsumerTag: {0}", queue.ExclusiveConsumerTag);
                    Console.WriteLine("ConsumerUtilization: {0}", queue.ConsumerUtilization);
                    Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                    Console.WriteLine("UnacknowledgedDeliveredMessagesInRam: {0}", queue.UnacknowledgedDeliveredMessagesInRam);
                    Console.WriteLine("UnacknowledgedDeliveredMessages: {0}", queue.UnacknowledgedDeliveredMessages);
                    Console.WriteLine("TotalMessagePagedOut: {0}", queue.TotalMessagePagedOut);
                    Console.WriteLine("TotalMessageBytesPagedOut: {0}", queue.TotalMessageBytesPagedOut);
                    Console.WriteLine("TotalBytesOfMessagesReadyForDelivery: {0}", queue.TotalBytesOfMessagesReadyForDelivery);
                    Console.WriteLine("TotalBytesOfMessagesDeliveredButUnacknowledged: {0}", queue.TotalBytesOfMessagesDeliveredButUnacknowledged);
                    Console.WriteLine("TotalBytesOfAllMessages: {0}", queue.TotalBytesOfAllMessages);
                    Console.WriteLine("State: {0}", queue.State);
                    Console.WriteLine("RecoverableSlaves: {0}", queue.RecoverableSlaves);
                    Console.WriteLine("RateOfReductions: {0}", queue.RateOfReductions?.Rate);
                    Console.WriteLine("RateOfMessagesUnacknowledged: {0}", queue.RateOfMessagesUnacknowledged?.Rate);
                    Console.WriteLine("RateOfMessagesReady: {0}", queue.RateOfMessagesReady?.Rate);
                    Console.WriteLine("RateOfMessage: {0}", queue.RateOfMessage?.Rate);
                    Console.WriteLine("Policy: {0}", queue.Policy);
                    Console.WriteLine("TotalTargetRam: {0}", queue.MessageRates?.TotalTargetRam);
                    Console.WriteLine("Q4: {0}", queue.MessageRates?.Q4);
                    Console.WriteLine("Q3: {0}", queue.MessageRates?.Q3);
                    Console.WriteLine("Q2: {0}", queue.MessageRates?.Q2);
                    Console.WriteLine("Q1: {0}", queue.MessageRates?.Q1);
                    Console.WriteLine("NextSequenceId: {0}", queue.MessageRates?.NextSequenceId);
                    Console.WriteLine("Mode: {0}", queue.MessageRates?.Mode);
                    Console.WriteLine("Length: {0}", queue.MessageRates?.Length);
                    Console.WriteLine("AvgIngressRate: {0}", queue.MessageRates?.AvgIngressRate);
                    Console.WriteLine("AvgEgressRate: {0}", queue.MessageRates?.AvgEgressRate);
                    Console.WriteLine("AvgAcknowledgementIngressRate: {0}", queue.MessageRates?.AvgAcknowledgementIngressRate);
                    Console.WriteLine("AvgAcknowledgementEgressRate: {0}", queue.MessageRates?.AvgAcknowledgementEgressRate);
                    Console.WriteLine("MessageBytesPersisted: {0}", queue.MessageBytesPersisted);
                    Console.WriteLine("MessageBytesInRam: {0}", queue.MessageBytesInRam);
                    Console.WriteLine("Memory: {0}", queue.Memory);
                }

                foreach (var user in definition.Users)
                {
                    Console.WriteLine("HashingAlgorithm: {0}", user.HashingAlgorithm);
                    Console.WriteLine("PasswordHash: {0}", user.PasswordHash);
                    Console.WriteLine("Tags: {0}", user.Tags);
                    Console.WriteLine("Username: {0}", user.Username);
                }

                foreach (var virtualHost in definition.VirtualHosts)
                {
                    Console.WriteLine("Name: {0}", virtualHost.Name);
                    Console.WriteLine("Tracing: {0}", virtualHost.Tracing);
                }
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}