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
namespace HareDu.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ChannelDebugExtensions
    {
        public static Task<ResultList<ChannelInfo>> ScreenDump(this Task<ResultList<ChannelInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Authentication Mechanism: {item.AuthenticationMechanism}");
                Console.WriteLine($"Confirm: {item.Confirm}");
                Console.WriteLine($"Connected At: {item.ConnectedAt}");
                Console.WriteLine("Connection");
                Console.WriteLine($"\tName: {item.ConnectionDetails?.Name}");
                Console.WriteLine($"\tPeer Host: {item.ConnectionDetails?.PeerHost}");
                Console.WriteLine($"\tPeer Port: {item.ConnectionDetails?.PeerPort}");
                Console.WriteLine($"Frame Max: {item.FrameMax}");
                Console.WriteLine("Garbage Collection");
                Console.WriteLine($"Full Sweep After: {item.GarbageCollectionDetails?.FullSweepAfter}");
                Console.WriteLine($"Minimum Heap Size: {item.GarbageCollectionDetails?.MinimumHeapSize}");
                Console.WriteLine($"Maximum Heap Size: {item.GarbageCollectionDetails?.MaximumHeapSize}");
                Console.WriteLine($"Minimum Binary Virtual Heap Size: {item.GarbageCollectionDetails?.MinimumBinaryVirtualHeapSize}");
                Console.WriteLine($"Minor: {item.GarbageCollectionDetails?.MinorGarbageCollection}");
                Console.WriteLine($"Global Prefetch Count: {item.GlobalPrefetchCount}");
                Console.WriteLine($"Host: {item.Host}");
                Console.WriteLine($"Idle Since: {item.IdleSince.ToString()}");
                Console.WriteLine($"Node: {item.Node}");
                Console.WriteLine($"Number: {item.Number}");
                Console.WriteLine("Peer Certificate");
                Console.WriteLine($"\tIssuer: {item.PeerCertificateIssuer}");
                Console.WriteLine($"\tSubject: {item.PeerCertificateSubject}");
                Console.WriteLine($"\tValidity: {item.PeerCertificateValidity}");
                Console.WriteLine($"Peer Host: {item.PeerHost}");
                Console.WriteLine($"Peer Port: {item.PeerPort}");
                Console.WriteLine($"Port: {item.Port}");
                Console.WriteLine($"Prefetch Count: {item.PrefetchCount}");
                Console.WriteLine($"Protocol: {item.Protocol}");
                Console.WriteLine($"Reduction: {item.ReductionDetails?.Rate} (rate)");
                Console.WriteLine($"Sent Pending: {item.SentPending}");
                Console.WriteLine("SSL");
                Console.WriteLine($"\tSSL: {item.Ssl}");
                Console.WriteLine($"\tCipher: {item.SslCipher}");
                Console.WriteLine($"\tHash: {item.SslHash}");
                Console.WriteLine($"\tKey Exchange: {item.SslKeyExchange}");
                Console.WriteLine($"\tProtocol: {item.SslProtocol}");
                Console.WriteLine($"State: {item.State}");
                Console.WriteLine($"Timeout: {item.Timeout}");
                Console.WriteLine($"Total Channels: {item.TotalChannels}");
                Console.WriteLine($"Total Consumers: {item.TotalConsumers}");
                Console.WriteLine($"Total Reductions: {item.TotalReductions}");
                Console.WriteLine($"Transactional: {item.Transactional}");
                Console.WriteLine($"Type: {item.Type}");
                Console.WriteLine($"Unacknowledged Messages: {item.UnacknowledgedMessages}");
                Console.WriteLine($"Uncommitted Acknowledgements: {item.UncommittedAcknowledgements}");
                Console.WriteLine($"Uncommitted Messages: {item.UncommittedMessages}");
                Console.WriteLine($"Unconfirmed Messages: {item.UnconfirmedMessages}");
                Console.WriteLine($"User: {item.User}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }
    }
}