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
namespace HareDu.Snapshotting.Model
{
    using System;
    using System.Collections.Generic;
    using Core.Model;
    using Newtonsoft.Json;

    public interface BrokerQueuesSnapshot :
        Snapshot
    {
        BrokerQueueChurnMetrics Churn { get; }
        
        IReadOnlyList<QueueSnapshot> Queues { get; }
    }

    public interface QueueUsageMetrics
    {
        
        [JsonProperty("backing_queue_status")]
        BackingQueueStatus MessageRates { get; }
        
        [JsonProperty("head_message_timestamp")]
        DateTimeOffset HeadMessageTimestamp { get; }
        
        [JsonProperty("message_bytes_persistent")]
        long MessageBytesPersisted { get; }
        
        [JsonProperty("message_bytes_unacknowledged")]
        long TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        
        [JsonProperty("message_bytes_ready")]
        long TotalBytesOfMessagesReadyForDelivery { get; }
        
        [JsonProperty("message_bytes")]
        long TotalBytesOfAllMessages { get; }
        
        [JsonProperty("messages_persistent")]
        long MessagesPersisted { get; }
        
        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GC { get; }
        
        [JsonProperty("state")]
        string State { get; }
        
        [JsonProperty("recoverable_slaves")]
        long RecoverableSlaves { get; }
        
        [JsonProperty("exclusive_consumer_tag")]
        string ExclusiveConsumerTag { get; }
        
        [JsonProperty("policy")]
        string Policy { get; }
        
        [JsonProperty("memory")]
        long Memory { get; }
    }
}