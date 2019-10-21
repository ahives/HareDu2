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
namespace HareDu.Snapshotting.Observers
{
    using System;
    using Extensions;
    using Model;

    public class DefaultConnectivitySnapshotConsoleLogger :
        IObserver<SnapshotContext<BrokerConnectivitySnapshot>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SnapshotContext<BrokerConnectivitySnapshot> value)
        {
            var connections = value.Snapshot.Select(x => x.Connections);
            
            for (int i = 0; i < connections.Count; i++)
            {
                Console.WriteLine("Connection => {0}", connections[0].Select(x => x.Identifier));
                Console.WriteLine("Channel Limit => {0}", connections[i].OpenChannelsLimit);
                Console.WriteLine("Channels => {0}", connections[i].Channels.Count);
                Console.WriteLine("Connections => {0} created | {1:0.0}/s, {2} closed | {3:0.0}/s",
                    value.Snapshot.Select(x => x.ConnectionsCreated).Select(x => x.Total),
                    value.Snapshot.Select(x => x.ConnectionsClosed).Select(x => x.Rate),
                    value.Snapshot.Select(x => x.ConnectionsClosed).Select(x => x.Total),
                    value.Snapshot.Select(x => x.ConnectionsClosed).Select(x => x.Rate));
                Console.WriteLine("Network Traffic");
                Console.WriteLine("\tSent: {0} packets | {1} | {2} msg/s",
                    connections[i].NetworkTraffic.Sent.Total,
                    $"{connections[i].NetworkTraffic.Sent.Bytes} bytes ({connections[i].NetworkTraffic.Sent.Bytes.ToByteString()})",
                    connections[i].NetworkTraffic.Sent.Rate);
                Console.WriteLine("\tReceived: {0} packets | {1} | {2} msg/s",
                    connections[i].NetworkTraffic.Received.Total,
                    $"{connections[i].NetworkTraffic.Received.Bytes} bytes ({connections[i].NetworkTraffic.Received.Bytes.ToByteString()})",
                    connections[i].NetworkTraffic.Received.Rate);

                Console.WriteLine("Channels");
                for (int j = 0; j < connections[i].Channels.Count; j++)
                {
                    Console.WriteLine("\tChannel => {0}, Consumers => {1}",
                        connections[i].Channels[j].Identifier,
                        connections[i].Channels[j].Consumers);
                }
                
                Console.WriteLine("****************************");
                Console.WriteLine();
            }
        }
    }
}