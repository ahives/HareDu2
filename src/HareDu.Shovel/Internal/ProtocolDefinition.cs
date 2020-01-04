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
namespace HareDu.Shovel.Internal
{
    using Newtonsoft.Json;

    public interface ProtocolDefinition
    {
        [JsonProperty("src-protocol")]
        string SourceProtocol { get; }
        
        [JsonProperty("dest-protocol")]
        string DestinationProtocol { get; }
        
        [JsonProperty("ack-mode")]
        string AcknowledgementMode { get; }
        
        [JsonProperty("reconnect-delay")]
        int ReconnectDelay { get; }
        
        [JsonProperty("src-prefetch-count")]
        int SourcePrefetchCount { get; }
        
        [JsonProperty("src-uri")]
        string SourceUri { get; }
        
        [JsonProperty("dest-uri")]
        string DestinationUri { get; }
        
        [JsonProperty("src-delete-after")]
        string DeleteShovelAfter { get; }
        
        [JsonProperty("dest-add-forward-headers")]
        bool DestinationAddForwardHeaders { get; }
        
        [JsonProperty("dest-add-timestamp-header")]
        bool DestinationAddTimestampHeader { get; }
    }
}