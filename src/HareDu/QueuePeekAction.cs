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
namespace HareDu
{
    using System;

    public interface QueuePeekAction
    {
        /// <summary>
        /// Specify the name of the queue.
        /// </summary>
        /// <param name="name">Name of the queue</param>
        void Queue(string name);

        /// <summary>
        /// Specify how to redeliver messages to the queue.
        /// </summary>
        /// <param name="configuration"></param>
        void Configure(Action<QueuePeekConfiguration> configuration);
        
        /// <summary>
        /// Specify where the queue lives (i.e. virtual host, etc.).
        /// </summary>
        /// <param name="target"></param>
        void Targeting(Action<QueuePeekTarget> target);
    }
}