namespace HareDu.Snapshotting
{
    using System;

    public class EmptySnapshotResult<T> :
        SnapshotResult<T>
        where T : Snapshot
    {
        public string Identifier { get; }
        public T Snapshot => throw new HareDuSnapshotException("There is no snapshot present.");
        public DateTimeOffset Timestamp => DateTimeOffset.Now;
    }
}