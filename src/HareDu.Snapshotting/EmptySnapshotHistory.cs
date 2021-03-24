namespace HareDu.Snapshotting
{
    using System.Collections.Generic;

    public class EmptySnapshotHistory<T> :
        SnapshotHistory<T>
        where T : Snapshot
    {
        public IReadOnlyList<SnapshotResult<T>> Results =>
            throw new HareDuSnapshotException("There are no snapshot result history. You returned an empty SnapshotLens.");
        
        public void PurgeAll()
        {
        }

        public void Purge<U>(SnapshotResult<U> result)
            where U : Snapshot
        {
        }
    }
}