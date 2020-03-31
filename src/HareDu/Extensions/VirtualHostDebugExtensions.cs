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

    public static class VirtualHostDebugExtensions
    {
        public static Task<ResultList<VirtualHostInfo>> ScreenDump(this Task<ResultList<VirtualHostInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"\tName: {item.Name}");
                Console.WriteLine($"\tTracing: {item.Tracing}");
                Console.WriteLine("\tMessages");
                Console.WriteLine($"\t\tMessages: {item.TotalMessages} (total), {item.MessagesDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tReady: {item.ReadyMessages} (total), {item.ReadyMessagesDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tUnacknowledged: {item.UnacknowledgedMessages} (total), {item.UnacknowledgedMessagesDetails?.Rate} (rate)");
                Console.WriteLine("\tPackets");
                Console.WriteLine($"\t\tSent: {item.PacketBytesSent} (bytes), {item.PacketBytesSentDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tReceived: {item.PacketBytesReceived} (bytes), {item.PacketBytesReceivedDetails?.Rate} (rate)");
                Console.WriteLine("\tMessage Stats");
                Console.WriteLine($"\t\tGets: {item.MessageStats?.TotalMessageGets} (total), {item.MessageStats?.MessageGetDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tAcknowledged: {item.MessageStats?.TotalMessagesAcknowledged} (total), {item.MessageStats?.MessagesAcknowledgedDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tConfirmed: {item.MessageStats?.TotalMessagesConfirmed} (total), {item.MessageStats?.MessagesConfirmedDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tDelivered: {item.MessageStats?.TotalMessagesDelivered} (total), {item.MessageStats?.MessageDeliveryDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tPublished: {item.MessageStats?.TotalMessagesPublished} (total), {item.MessageStats?.MessagesPublishedDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tRedelivered: {item.MessageStats?.TotalMessagesRedelivered} (total), {item.MessageStats?.MessagesRedeliveredDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tUnroutable: {item.MessageStats?.TotalUnroutableMessages} (total), {item.MessageStats?.UnroutableMessagesDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tDelivery Gets: {item.MessageStats?.TotalMessageDeliveryGets} (total), {item.MessageStats?.MessageDeliveryGetDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tDelivered Without Ack: {item.MessageStats?.TotalMessageDeliveredWithoutAck} (total), {item.MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate} (rate)");
                Console.WriteLine($"\t\tGets Without Ack: {item.MessageStats?.TotalMessageGetsWithoutAck} (total), {item.MessageStats?.MessageGetsWithoutAckDetails?.Rate} (rate)");

                foreach (var pair in item.ClusterState)
                {
                    Console.WriteLine($"\t\tKey: {pair.Key}, Value: {pair.Value}");
                }
                
                Console.WriteLine("\t-------------------");
                Console.WriteLine();
            }

            return result;
        }
    }
}