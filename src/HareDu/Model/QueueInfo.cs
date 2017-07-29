// Copyright 2013-2017 Albert L. Hives
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
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface QueueInfo
    {
        [JsonProperty("messages_details")]
        Rate RateOfMessage { get; }
        
        [JsonProperty("messages")]
        long Messages { get; }
        
        [JsonProperty("messages_unacknowledged_details")]
        Rate RateOfMessagesUnacknowledged { get; }
        
        [JsonProperty("messages_unacknowledged")]
        long MessagesUnacknowledged { get; }
        
        [JsonProperty("messages_ready_details")]
        Rate RateOfMessagesReady { get; }
        
        [JsonProperty("messages_ready")]
        long TotalMessages { get; }
        
        [JsonProperty("reductions_details")]
        Rate RateOfReductions { get; }
        
        [JsonProperty("reductions")]
        long TotalReductions { get; }
        
        [JsonProperty("node")]
        string Node { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonProperty("exclusive")]
        bool Exclusive { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("message_bytes_paged_out")]
        long TotalMessageBytesPagedOut { get; }
        
        [JsonProperty("messages_paged_out")]
        long TotalMessagePagedOut { get; }
        
        [JsonProperty("backing_queue_status")]
        QueueStatus MessageRates { get; }
        
        [JsonProperty("head_message_timestamp")]
        DateTimeOffset HeadMessageTimestamp { get; }
        
        [JsonProperty("message_bytes_persistent")]
        long MessageBytesPersisted { get; }
        
        [JsonProperty("message_bytes_ram")]
        long MessageBytesInRam { get; }
        
        [JsonProperty("message_bytes_unacknowledged")]
        long MessageBytesUnacknowledged { get; }
        
        [JsonProperty("message_bytes_ready")]
        long MessageBytesReady { get; }
        
        [JsonProperty("message_bytes")]
        long MessageBytes { get; }
        
        [JsonProperty("messages_persistent")]
        long MessagePersisted { get; }
        
        [JsonProperty("messages_unacknowledged_ram")]
        long MessagesInRamUnacknowledged { get; }
        
        [JsonProperty("messages_ready_ram")]
        long MessageInRamReady { get; }
        
        [JsonProperty("messages_ram")]
        long MessageInRam { get; }
        
        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GC { get; }
        
        [JsonProperty("state")]
        string State { get; }
        
        [JsonProperty("recoverable_slaves")]
        long RecoverableSlaves { get; }
        
        [JsonProperty("consumers")]
        long Consumers { get; }
        
        [JsonProperty("exclusive_consumer_tag")]
        string ExclusiveConsumerTag { get; }
        
        [JsonProperty("policy")]
        string Policy { get; }
        
        [JsonProperty("consumer_utilisation")]
        string ConsumerUtilization { get; }
        
        [JsonProperty("idle_since")]
        DateTimeOffset IdleSince { get; }
        
        [JsonProperty("memory")]
        long Memory { get; }
    }
}