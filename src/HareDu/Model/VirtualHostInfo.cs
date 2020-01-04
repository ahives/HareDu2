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
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface VirtualHostInfo :
        ResourceSummary
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("tracing")]
        string Tracing { get; }
        
        [JsonProperty("cluster_state")]
        IDictionary<string, string> ClusterState { get; }

        [JsonProperty("message_stats")]
        VirtualHostMessageStats MessageStats { get; }

        [JsonProperty("recv_oct")]
        ulong PacketBytesReceived { get; }

        [JsonProperty("recv_oct_details")]
        Rate RateOfPacketBytesReceived { get; }

        [JsonProperty("send_oct")]
        ulong PacketBytesSent { get; }

        [JsonProperty("send_oct_details")]
        Rate RateOfPacketBytesSent { get; }
        
        [JsonProperty("messages_details")]
        Rate RateOfMessages { get; }
        
        [JsonProperty("messages")]
        ulong TotalMessages { get; }
        
        [JsonProperty("messages_unacknowledged_details")]
        Rate RateOfUnacknowledgedMessages { get; }
        
        [JsonProperty("messages_unacknowledged")]
        ulong UnacknowledgedMessages { get; }
        
        [JsonProperty("messages_ready_details")]
        Rate RateOfReadyMessages { get; }
        
        [JsonProperty("messages_ready")]
        ulong ReadyMessages { get; }
    }
}