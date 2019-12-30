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
namespace HareDu.Diagnostics
{
    using System.IO;
    using Core.Extensions;
    using Snapshotting;

    public static class PersistenceExtensions
    {
        public static bool PersistJson(this ScannerResult report, string path)
        {
            string file = $"{path}/report_{report.Identifier.ToString()}.json";
            if (File.Exists(file))
                return false;
            
            File.WriteAllText(file, report.ToJsonString());
            return true;
        }
    }
}