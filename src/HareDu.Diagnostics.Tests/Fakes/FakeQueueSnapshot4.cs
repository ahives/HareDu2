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
        public string Node { get; }
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

            public ulong Total { get; }
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