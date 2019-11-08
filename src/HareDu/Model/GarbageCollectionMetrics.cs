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
namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface GarbageCollectionMetrics
    {
        [JsonProperty("connection_closed")]
        ulong ConnectionsClosed { get; }

        [JsonProperty("channel_closed")]
        ulong ChannelsClosed { get; }

        [JsonProperty("consumer_deleted")]
        ulong ConsumersDeleted { get; }
        
        [JsonProperty("exchange_deleted")]
        ulong ExchangesDeleted { get; }

        [JsonProperty("queue_deleted")]
        ulong QueuesDeleted { get; }

        [JsonProperty("vhost_deleted")]
        ulong VirtualHostsDeleted { get; }

        [JsonProperty("node_node_deleted")]
        ulong NodesDeleted { get; }

        [JsonProperty("channel_consumer_deleted")]
        ulong ChannelConsumersDeleted { get; }
    }
}