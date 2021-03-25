namespace HareDu.Snapshotting.Extensions
{
    using Persistence;

    public static class SnapshotHistoryExtensions
    {
        public static void Flush<T>(this SnapshotHistory<T> history, ISnapshotWriter writer, string path)
            where T : Snapshot
        {
            for (int i = 0; i < history.Results.Count; i++)
            {
                writer.TrySave(history.Results[i], $"snapshot_{history.Results[i].Identifier}.json", path);
            }
        }

        public static void Flush<T>(this SnapshotHistory<T> history, ISnapshotWriter writer, string file, string path)
            where T : Snapshot
        {
            for (int i = 0; i < history.Results.Count; i++)
            {
                writer.TrySave(history.Results[i], $"{file}_{history.Results[i].Identifier}.json", path);
            }
        }
    }
}