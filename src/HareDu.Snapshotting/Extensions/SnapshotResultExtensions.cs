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
        /// <param name="timeline"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SnapshotResult<T> MostRecent<T>(this SnapshotTimeline<T> timeline)
            where T : Snapshot
            => timeline.IsNull() || timeline.Results.IsNull() || !timeline.Results.Any()
                ? new EmptySnapshotResult<T>()
                : timeline.Results.Last();

        public static void Save<T>(this SnapshotResult<T> result, ISnapshotWriter writer, string file, string path)
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

            writer.TrySave(result, file, path);
        }
    }
}