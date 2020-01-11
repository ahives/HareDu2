// Copyright 2013-2020 Albert L. Hives
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
    using Persistence;

    public static class SnapshotTimelineExtensions
    {
        public static void Flush<T>(this SnapshotTimeline<T> timeline, ISnapshotWriter writer, string path)
            where T : Snapshot
        {
            for (int i = 0; i < timeline.Results.Count; i++)
            {
                writer.TrySave(timeline.Results[i], $"snapshot_{timeline.Results[i].Identifier}.json", path);
            }
        }

        public static void Flush<T, U>(this SnapshotTimeline<T> timeline, ISnapshotWriter writer, string file, string path)
            where T : Snapshot
        {
            for (int i = 0; i < timeline.Results.Count; i++)
            {
                writer.TrySave(timeline.Results[i], $"{file}_{timeline.Results[i].Identifier}.json", path);
            }
        }
    }
}