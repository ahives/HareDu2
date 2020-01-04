// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeChannelSnapshot2 :
        ChannelSnapshot
    {
        public FakeChannelSnapshot2(uint prefetchCount)
        {
            PrefetchCount = prefetchCount;
        }

        public uint PrefetchCount { get; }
        public ulong UncommittedAcknowledgements { get; }
        public ulong UncommittedMessages { get; }
        public ulong UnconfirmedMessages { get; }
        public ulong UnacknowledgedMessages { get; }
        public ulong Consumers { get; }
        public string Identifier { get; }
        public string ConnectionIdentifier { get; }
        public string Node { get; }
        public QueueOperationMetrics QueueOperations { get; }
    }
}