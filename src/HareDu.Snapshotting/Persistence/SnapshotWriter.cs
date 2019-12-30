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
namespace HareDu.Snapshotting.Persistence
{
    using System.IO;
    using Core.Extensions;

    public class SnapshotWriter :
        ISnapshotWriter
    {
        public bool TrySave<T>(SnapshotResult<T> result, string file, string path)
            where T : Snapshot
        {
            if (Directory.Exists(path))
                return Write(result, file, path);
            
            var dir = Directory.CreateDirectory(path);

            return dir.Exists ? Write(result, file, path) : Write(result, file, path);
        }

        bool Write<T>(SnapshotResult<T> result, string file, string path)
            where T : Snapshot
        {
            string fullPath = $"{path}/{file}";
            
            if (File.Exists(fullPath))
                return false;

            File.WriteAllText(fullPath, result.ToJsonString());
            return true;
        }
    }
}