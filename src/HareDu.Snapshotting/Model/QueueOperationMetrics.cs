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
    public interface QueueOperationMetrics
    {
        /// <summary>
        /// Total number of publish operations sent to all queues and rate at which they were sent in msg/s.
        /// </summary>
        QueueOperation Incoming { get; }
        
        /// <summary>
        /// Total number of "Get" operations sent to all queue with acknowledgement and the corresponding rate at which they were sent in msg/s.
        /// </summary>
        QueueOperation Gets { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the channel without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueOperation GetsWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueueOperation Delivered { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueOperation DeliveredWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages delivered to consumers via "Get" operations and the corresponding rate in msg/s. 
        /// </summary>
        QueueOperation DeliveredGets { get; }
        
        /// <summary>
        /// Total/rate (msg/s) of messages that were redelivered to consumers.
        /// </summary>
        QueueOperation Redelivered { get; }
        
        /// <summary>
        /// Total/rate (msg/s) of messages that were acknowledged to have been consumed.
        /// </summary>
        QueueOperation Acknowledged { get; }
        
        /// <summary>
        /// Total number of not routed operations.
        /// </summary>
        QueueOperation NotRouted { get; }
    }
}