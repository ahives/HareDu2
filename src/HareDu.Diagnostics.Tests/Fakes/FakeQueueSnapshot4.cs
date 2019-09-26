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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using Snapshotting.Model;

    public class FakeQueueSnapshot4 :
        QueueSnapshot
    {
        public FakeQueueSnapshot4(ulong total)
        {
            Memory = new QueueMemoryDetailsImpl(total);
        }

        public string Identifier { get; }
        public string VirtualHost { get; }
        public string NodeIdentifier { get; }
        public QueueChurnMetrics Messages { get; }
        public QueueMemoryDetails Memory { get; }
        public QueueInternals Internals { get; }
        public ulong Consumers { get; }
        public decimal ConsumerUtilization { get; }
        public DateTimeOffset IdleSince { get; }

            
        class QueueMemoryDetailsImpl :
            QueueMemoryDetails
        {
            public QueueMemoryDetailsImpl(ulong total)
            {
                PagedOut = new PagedOutImpl(total);
            }

            public long Total { get; }
            public PagedOut PagedOut { get; }
            public RAM RAM { get; }

                
            class PagedOutImpl :
                PagedOut
            {
                public PagedOutImpl(ulong total)
                {
                    Total = total;
                }

                public ulong Total { get; }
                public ulong Bytes { get; }
            }
        }
    }
}