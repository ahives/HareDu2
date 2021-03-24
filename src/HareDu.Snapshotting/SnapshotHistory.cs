namespace HareDu.Snapshotting
{
    using System.Collections.Generic;

    public interface SnapshotHistory<out T>
        where T : Snapshot
    {
        IReadOnlyList<SnapshotResult<T>> Results { get; }

        void PurgeAll();

        void Purge<U>(SnapshotResult<U> result)
            where U : Snapshot;
    }
}