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
namespace HareDu
{
    public interface QueuePeekConfiguration
    {
        /// <summary>
        /// Specify how many messages to take from the queue.
        /// </summary>
        /// <param name="count"></param>
        void Take(uint count);

        /// <summary>
        /// Specify that after messages are popped from the queue to requeue them.
        /// </summary>
        void PutBackWhenFinished();

        /// <summary>
        /// Specify how to encode messages when requeued. 
        /// </summary>
        /// <param name="encoding"></param>
        void Encoding(MessageEncoding encoding);

        /// <summary>
        /// Specify the size of messages in bytes that are acceptable before having to truncate.
        /// </summary>
        /// <param name="bytes"></param>
        void TruncateIfAbove(uint bytes);
    }
}