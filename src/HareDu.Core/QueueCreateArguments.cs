﻿// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Core
{
    public interface QueueCreateArguments
    {
        /// <summary>
        /// Add a new argument.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string arg, T value);
        
        /// <summary>
        /// Set 'x-expires' argument.
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to set queue expiration</param>
        void SetQueueExpiration(long milliseconds);
        
        /// <summary>
        /// Set 'x-message-ttl' argument.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetPerQueuedMessageExpiration(long milliseconds);
        
        /// <summary>
        /// Set 'x-dead-letter-exchange' argument.
        /// </summary>
        /// <param name="exchange"></param>
        void SetDeadLetterExchange(string exchange);
        
        /// <summary>
        /// Set 'x-dead-letter-routing-key' argument.
        /// </summary>
        /// <param name="routingKey"></param>
        void SetDeadLetterExchangeRoutingKey(string routingKey);
        
        /// <summary>
        /// Set 'alternate-exchange' argument.
        /// </summary>
        /// <param name="exchange"></param>
        void SetAlternateExchange(string exchange);
    }
}