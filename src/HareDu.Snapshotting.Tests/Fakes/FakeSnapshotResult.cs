namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;

    public class FakeSnapshotResult<T> :
        SnapshotResult<T>
        where T : Snapshot
    {
        public FakeSnapshotResult(T snapshot, string identifier)
        {
            Identifier = identifier;
            Snapshot = snapshot;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public string Identifier { get; }
        public T Snapshot { get; }
        public DateTimeOffset Timestamp { get; }
    }
}