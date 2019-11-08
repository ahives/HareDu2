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

    public interface ChurnRates
    {
        [JsonProperty("channel_closed")]
        ulong TotalChannelsClosed { get; }
        
        [JsonProperty("channel_closed_details")]
        Rate ClosedChannelDetails { get; }

        [JsonProperty("channel_created")]
        ulong TotalChannelsCreated { get; }
        
        [JsonProperty("channel_created_details")]
        Rate CreatedChannelDetails { get; }

        [JsonProperty("connection_closed")]
        ulong TotalConnectionsClosed { get; }
        
        [JsonProperty("connection_closed_details")]
        Rate ClosedConnectionDetails { get; }

        [JsonProperty("connection_created")]
        ulong TotalConnectionsCreated { get; }
        
        [JsonProperty("connection_created_details")]
        Rate CreatedConnectionDetails { get; }

        [JsonProperty("queue_created")]
        ulong TotalQueuesCreated { get; }
        
        [JsonProperty("queue_created_details")]
        Rate CreatedQueueDetails { get; }

        [JsonProperty("queue_declared")]
        ulong TotalQueuesDeclared { get; }
        
        [JsonProperty("queue_declared_details")]
        Rate DeclaredQueueDetails { get; }

        [JsonProperty("queue_deleted")]
        ulong TotalQueuesDeleted { get; }
        
        [JsonProperty("queue_deleted_details")]
        Rate DeletedQueueDetails { get; }
    }
}