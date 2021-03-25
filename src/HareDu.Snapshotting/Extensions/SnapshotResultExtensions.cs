namespace HareDu.Snapshotting.Extensions
{
    using System;
    using System.Linq;
    using Core.Extensions;
    using Persistence;

    public static class SnapshotResultExtensions
    {
        /// <summary>
        /// Returns the most recent snapshot in the timeline.
        /// </summary>
        /// <param name="history"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SnapshotResult<T> MostRecent<T>(this SnapshotHistory<T> history)
            where T : Snapshot
            => history.IsNull() || history.Results.IsNull() || !history.Results.Any()
                ? new EmptySnapshotResult<T>()
                : history.Results.Last();

        /// <summary>
        /// Attempts to save the <see cref="SnapshotResult{T}"/> to disk.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="writer"></param>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Save<T>(this SnapshotResult<T> result, ISnapshotWriter writer, string file, string path)
            where T : Snapshot
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (writer.IsNull())
                throw new ArgumentNullException(nameof(writer));

            if (result.IsNull())
                throw new ArgumentNullException(nameof(result));

            return writer.TrySave(result, file, path);
        }
    }
}