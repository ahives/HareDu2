namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using Internal.Resources;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ChannelTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Test()
        {
            Result<ChannelInfo> result = await Client
                .Resource<Channel>()
                .GetAll();
            
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
            Console.WriteLine(result.ToJsonString());
        }
        
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_channels()
        {
            Result<ChannelInfo> result = await Client
                .Resource<Channel>()
                .GetAll();

            if (result.HasData)
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
    }
}