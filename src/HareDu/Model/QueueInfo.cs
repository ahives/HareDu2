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
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface QueueInfo
    {
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
        
        [JsonProperty("reductions_details")]
        Rate RateOfReductions { get; }
        
        [JsonProperty("reductions")]
        long TotalReductions { get; }
        
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
        
        [JsonProperty("node")]
        string Node { get; }
        
        [JsonProperty("message_bytes_paged_out")]
        ulong TotalMessageBytesPagedOut { get; }
        
        [JsonProperty("messages_paged_out")]
        ulong TotalMessagesPagedOut { get; }
        
        [JsonProperty("backing_queue_status")]
        BackingQueueStatus BackingQueueStatus { get; }
        
        [JsonProperty("head_message_timestamp")]
        DateTimeOffset HeadMessageTimestamp { get; }
        
        [JsonProperty("message_bytes_persistent")]
        ulong MessageBytesPersisted { get; }
        
        [JsonProperty("message_bytes_ram")]
        ulong MessageBytesInRam { get; }
        
        [JsonProperty("message_bytes_unacknowledged")]
        ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        
        [JsonProperty("message_bytes_ready")]
        ulong TotalBytesOfMessagesReadyForDelivery { get; }
        
        [JsonProperty("message_bytes")]
        ulong TotalBytesOfAllMessages { get; }
        
        [JsonProperty("messages_persistent")]
        ulong MessagesPersisted { get; }
        
        [JsonProperty("messages_unacknowledged_ram")]
        ulong UnacknowledgedMessagesInRam { get; }
        
        [JsonProperty("messages_ready_ram")]
        ulong MessagesReadyForDeliveryInRam { get; }
        
        [JsonProperty("messages_ram")]
        ulong MessagesInRam { get; }
        
        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GC { get; }
        
        [JsonProperty("state")]
        string State { get; }
        
        [JsonProperty("recoverable_slaves")]
        long RecoverableSlaves { get; }
        
        [JsonProperty("consumers")]
        ulong Consumers { get; }
        
        [JsonProperty("exclusive_consumer_tag")]
        string ExclusiveConsumerTag { get; }
        
        [JsonProperty("policy")]
        string Policy { get; }
        
        [JsonProperty("consumer_utilisation")]
        decimal ConsumerUtilization { get; }
        
        [JsonProperty("idle_since")]
        DateTimeOffset IdleSince { get; }
        
        [JsonProperty("memory")]
        long Memory { get; }
        
        [JsonProperty("message_stats")]
        QueueMessageStats MessageStats { get; }
    }
}