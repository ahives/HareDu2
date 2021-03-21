namespace HareDu.Snapshotting.Persistence
{
    public interface ISnapshotWriter
    {
        bool TrySave<T>(SnapshotResult<T> result, string file, string path)
            where T : Snapshot;
    }
}