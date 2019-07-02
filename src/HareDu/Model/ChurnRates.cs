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
        int TotalChannelsClosed { get; }
        
        [JsonProperty("channel_closed_details")]
        ClosedChannelDetails ClosedChannelDetails { get; }

        [JsonProperty("channel_created")]
        int TotalChannelsCreated { get; }
        
        [JsonProperty("channel_created_details")]
        CreatedChannelDetails CreatedChannelDetails { get; }

        [JsonProperty("connection_closed")]
        int TotalConnectionsClosed { get; }
        
        [JsonProperty("connection_closed_details")]
        ClosedConnectionDetails ClosedConnectionDetails { get; }

        [JsonProperty("connection_created")]
        int TotalConnectionsCreated { get; }
        
        [JsonProperty("connection_created_details")]
        CreatedConnectionDetails CreatedConnectionDetails { get; }

        [JsonProperty("queue_created")]
        int TotalQueuesCreated { get; }
        
        [JsonProperty("queue_created_details")]
        CreatedQueueDetails CreatedQueueDetails { get; }

        [JsonProperty("queue_declared")]
        int TotalQueuesDeclared { get; }
        
        [JsonProperty("queue_declared_details")]
        DeclaredQueueDetails DeclaredQueueDetails { get; }

        [JsonProperty("queue_deleted")]
        int TotalQueuesDeleted { get; }
        
        [JsonProperty("queue_deleted_details")]
        DeletedQueueDetails DeletedQueueDetails { get; }
    }
}