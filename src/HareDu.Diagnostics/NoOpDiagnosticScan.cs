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
namespace HareDu.Diagnostics
{
    using System.Collections.Generic;
    using Core.Extensions;
    using Scans;
    using Snapshotting;

    public class NoOpDiagnosticScan<T> :
        DiagnosticScan<T>
        where T : Snapshot
    {
        public string Identifier => GetType().GetIdentifier();

        public IReadOnlyList<DiagnosticProbeResult> Scan(T snapshot) => DiagnosticCache.EmptyProbeResults;
    }
}