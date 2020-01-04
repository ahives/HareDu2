﻿// Copyright 2013-2020 Albert L. Hives
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

    public interface ConsumerInfo
    {
        [JsonProperty("prefetch_count")]
        ulong PreFetchCount { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonProperty("ack_required")]
        bool AcknowledgementRequired { get; }
        
        [JsonProperty("exclusive")]
        bool Exclusive { get; }
        
        [JsonProperty("consumer_tag")]
        string ConsumerTag { get; }
        
        [JsonProperty("channel_details")]
        ChannelDetails ChannelDetails { get; }
        
        [JsonProperty("queue")]
        QueueConsumerDetails QueueConsumerDetails { get; }
    }
}