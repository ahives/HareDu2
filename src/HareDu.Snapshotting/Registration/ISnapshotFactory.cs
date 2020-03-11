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
namespace HareDu.Snapshotting.Registration
{
    public interface ISnapshotFactory
    {
        /// <summary>
        /// Returns a snapshot lens if present. Otherwise, it returns a default lens if none is present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        SnapshotLens<T> Lens<T>()
            where T : Snapshot;

        /// <summary>
        /// Caches the specified snapshot lens so that every time <see cref="T"/> is accessed the proper lens will be loaded.
        /// </summary>
        /// <param name="lens"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISnapshotFactory Register<T>(SnapshotLens<T> lens)
            where T : Snapshot;
    }
}