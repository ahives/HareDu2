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
    using Core.Model;
    using Newtonsoft.Json;

    public interface QueueMetrics
    {
        string Name { get; }
        
        string VirtualHost { get; }
        
        QueuedMessageMetrics Messages { get; }
        
        QueueInternalMetrics Internals { get; }
        
        /// <summary>
        /// Combination of the total/rate of ready and unacknowledged messages in the queue.
        /// </summary>
        QueuedMessageDetails Depth { get; }
    }

    public interface QueueUsageMetrics
    {
        
        [JsonProperty("backing_queue_status")]
        QueueStatus MessageRates { get; }
        
        [JsonProperty("head_message_timestamp")]
        DateTimeOffset HeadMessageTimestamp { get; }
        
        [JsonProperty("message_bytes_persistent")]
        long MessageBytesPersisted { get; }
        
        [JsonProperty("message_bytes_ram")]
        long MessageBytesInRam { get; }
        
        [JsonProperty("message_bytes_unacknowledged")]
        long TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        
        [JsonProperty("message_bytes_ready")]
        long TotalBytesOfMessagesReadyForDelivery { get; }
        
        [JsonProperty("message_bytes")]
        long TotalBytesOfAllMessages { get; }
        
        [JsonProperty("messages_persistent")]
        long MessagesPersisted { get; }
        
        [JsonProperty("messages_unacknowledged_ram")]
        long UnacknowledgedMessagesInRam { get; }
        
        [JsonProperty("messages_ready_ram")]
        long MessagesReadyForDeliveryInRam { get; }
        
        [JsonProperty("messages_ram")]
        long MessagesInRam { get; }
        
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

    public interface QueueInternalMetrics
    {
        Something Something { get; }
        
        long Q1 { get; }
        
        [JsonProperty("q2")]
        long Q2 { get; }
        
        [JsonProperty("delta")]
        IList<object> Delta { get; }
        
        [JsonProperty("q3")]
        long Q3 { get; }
        
        [JsonProperty("q4")]
        long Q4 { get; }
        
        [JsonProperty("len")]
        long Length { get; }
        
        [JsonProperty("target_ram_count")]
        string TotalTargetRam { get; }
        
        [JsonProperty("next_seq_id")]
        long NextSequenceId { get; }
        
        [JsonProperty("avg_ingress_rate")]
        decimal AvgIngressRate { get; }
        
        [JsonProperty("avg_egress_rate")]
        decimal AvgEgressRate { get; }
        
        [JsonProperty("avg_ack_ingress_rate")]
        decimal AvgAcknowledgementIngressRate { get; }
        
        [JsonProperty("avg_ack_egress_rate")]
        decimal AvgAcknowledgementEgressRate { get; }
    }

    public interface Something
    {
        SomeMetric Reductions { get; }
        
        long PagedOut { get; }
        
        long PagedOutBytes { get; }
    }

    public interface SomeMetric
    {
        long Total { get; }
        
        decimal Rate { get; }
    }

    public interface QueuedMessageMetrics
    {
        /// <summary>
        /// Total number of messages persisted to Mnesia.
        /// </summary>
        long Persisted { get; }
        
        /// <summary>
        /// Total number of messages published to the queue and rate at which they were sent and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Incoming { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue that were consumed but not acknowledged and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Unacknowledged { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue ready to be delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Ready { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue with acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Gets { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails GetsWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Delivered { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails DeliveredWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages delivered to consumers via "Get" operations and the corresponding rate in msg/s. 
        /// </summary>
        QueuedMessageDetails DeliveredGets { get; }
        
        /// <summary>
        /// Total number of messages that were redelivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Redelivered { get; }
        
        /// <summary>
        /// Total number of messages that were acknowledged to have been consumed and the corresponding rate in msg/s.
        /// </summary>
        QueuedMessageDetails Acknowledged { get; }
    }

    public interface QueuedMessageDetails
    {
        long Total { get; }
        
        decimal Rate { get; }
    }
}