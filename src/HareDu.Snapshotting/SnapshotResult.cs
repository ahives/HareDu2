namespace HareDu.Snapshotting
{
    using System;

    public interface SnapshotResult<out T>
        where T : Snapshot
    {
        string Identifier { get; }

        T Snapshot { get; }

        DateTimeOffset Timestamp { get; }
    }
}