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
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_connections()
        {
            var result = await Client
                .Object<Connection>()
                .GetAll();

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
                Console.WriteLine("BytessReceived: {0}", connection.PacketBytesReceived);
                Console.WriteLine("PacketsReceived: {0}", connection.PacketsReceived);
                Console.WriteLine("PeerCertificateIssuer: {0}", connection.PeerCertificateIssuer);
                Console.WriteLine("PeerCertificateSubject: {0}", connection.PeerCertificateSubject);
                Console.WriteLine("PeerHost: {0}", connection.PeerHost);
                Console.WriteLine("PeerPort: {0}", connection.PeerPort);
                Console.WriteLine("Port: {0}", connection.Port);
                Console.WriteLine("RateOfOctetsRecevied: {0}", connection.RateOfPacketBytesReceived.Rate);
                Console.WriteLine("RateOfOctetsSent: {0}", connection.RateOfPacketBytesSent.Rate);
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
    }
}