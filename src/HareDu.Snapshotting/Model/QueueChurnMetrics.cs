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
    public interface QueueChurnMetrics
    {
        /// <summary>
        /// Total number of messages published to the queue and rate at which they were sent and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Incoming { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue that were consumed but not acknowledged and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Unacknowledged { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue ready to be delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Ready { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue with acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Gets { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth GetsWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Delivered { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth DeliveredWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages delivered to consumers via "Get" operations and the corresponding rate in msg/s. 
        /// </summary>
        QueueDepth DeliveredGets { get; }
        
        /// <summary>
        /// Total number/rate (msg/s) of messages that were redelivered to consumers.
        /// </summary>
        QueueDepth Redelivered { get; }
        
        /// <summary>
        /// Total number of messages that were acknowledged to have been consumed and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Acknowledged { get; }
        
        /// <summary>
        /// Total number of messages that 
        /// </summary>
        QueueDepth Aggregate { get; }
    }
}