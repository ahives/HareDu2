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
namespace HareDu
{
    using System;

    public interface QueueConfiguration
    {
        /// <summary>
        /// Specify whether the queue is durable. By default this is set to false, which means that it will created as a RAM queue.
        /// </summary>
        void IsDurable();

        /// <summary>
        /// Specify arguments for the queue.
        /// </summary>
        /// <param name="arguments">Pre-defined arguments applied to the definition of the queue.</param>
        void HasArguments(Action<QueueCreateArguments> arguments);

        /// <summary>
        /// Specify whether the queue is deleted when there are no consumers.
        /// </summary>
        void AutoDeleteWhenNotInUse();
    }
}