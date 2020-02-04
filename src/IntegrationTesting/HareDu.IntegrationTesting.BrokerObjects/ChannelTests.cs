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
namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class ChannelTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test, Explicit]
        public async Task Test()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>()
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
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll();

            if (result.HasData)
            {
                foreach (var channel in result.Select(x => x.Data))
                {
                    Console.WriteLine("Name: {0}", channel.Name);
                    Console.WriteLine("PrefetchCount: {0}", channel.PrefetchCount);
                    Console.WriteLine("AuthenticationMechanism: {0}", channel.AuthenticationMechanism);
                    Console.WriteLine("Confirm: {0}", channel.Confirm);
                    Console.WriteLine("ConnectedAt: {0}", channel.ConnectedAt);
                    Console.WriteLine("ConnectionDetailsName: {0}", channel.ConnectionDetails.Name);
                    Console.WriteLine("PeerHost: {0}", channel.ConnectionDetails.PeerHost);
                    Console.WriteLine("PeerPort: {0}", channel.ConnectionDetails.PeerPort);
                    Console.WriteLine("FrameMax: {0}", channel.FrameMax);
                    Console.WriteLine("FullSweepAfter: {0}", channel.GarbageCollectionDetails.FullSweepAfter);
                    Console.WriteLine("MaximumHeapSize: {0}", channel.GarbageCollectionDetails.MaximumHeapSize);
                    Console.WriteLine("MinimumBinaryVirtualHeapSize: {0}",
                        channel.GarbageCollectionDetails.MinimumBinaryVirtualHeapSize);
                    Console.WriteLine("MinimumHeapSize: {0}", channel.GarbageCollectionDetails.MinimumHeapSize);
                    Console.WriteLine("MinorGarbageCollection: {0}",
                        channel.GarbageCollectionDetails.MinorGarbageCollection);
                    Console.WriteLine("GlobalPrefetchCount: {0}", channel.GlobalPrefetchCount);
                    Console.WriteLine("Host: {0}", channel.Host);
                    Console.WriteLine("IdleSince: {0}", channel.IdleSince.ToString());
                    Console.WriteLine("Node: {0}", channel.Node);
                    Console.WriteLine("Number: {0}", channel.Number);
                    Console.WriteLine("PeerCertificateIssuer: {0}", channel.PeerCertificateIssuer);
                    Console.WriteLine("PeerCertificateSubject: {0}", channel.PeerCertificateSubject);
                    Console.WriteLine("PeerCertificateValidity: {0}", channel.PeerCertificateValidity);
                    Console.WriteLine("PeerHost: {0}", channel.PeerHost);
                    Console.WriteLine("PeerPort: {0}", channel.PeerPort);
                    Console.WriteLine("Port: {0}", channel.Port);
                    Console.WriteLine("PrefetchCount: {0}", channel.PrefetchCount);
                    Console.WriteLine("Protocol: {0}", channel.Protocol);
                    Console.WriteLine("RateOfReduction: {0}", channel.ReductionDetails.Rate);
                    Console.WriteLine("SentPending: {0}", channel.SentPending);
                    Console.WriteLine("Ssl: {0}", channel.Ssl);
                    Console.WriteLine("SslCipher: {0}", channel.SslCipher);
                    Console.WriteLine("SslHash: {0}", channel.SslHash);
                    Console.WriteLine("SslKeyExchange: {0}", channel.SslKeyExchange);
                    Console.WriteLine("SslProtocol: {0}", channel.SslProtocol);
                    Console.WriteLine("State: {0}", channel.State);
                    Console.WriteLine("Timeout: {0}", channel.Timeout);
                    Console.WriteLine("TotalChannels: {0}", channel.TotalChannels);
                    Console.WriteLine("TotalConsumers: {0}", channel.TotalConsumers);
                    Console.WriteLine("TotalReductions: {0}", channel.TotalReductions);
                    Console.WriteLine("Transactional: {0}", channel.Transactional);
                    Console.WriteLine("Type: {0}", channel.Type);
                    Console.WriteLine("UnacknowledgedMessages: {0}", channel.UnacknowledgedMessages);
                    Console.WriteLine("UncommittedAcknowledgements: {0}", channel.UncommittedAcknowledgements);
                    Console.WriteLine("UncommittedMessages: {0}", channel.UncommittedMessages);
                    Console.WriteLine("UnconfirmedMessages: {0}", channel.UnconfirmedMessages);
                    Console.WriteLine("User: {0}", channel.User);
                    Console.WriteLine("VirtualHost: {0}", channel.VirtualHost);
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
        }
    }
}