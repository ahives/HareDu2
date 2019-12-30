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
    using System;
    using System.Collections.Generic;
    using MassTransit;
    using Scanning;
    using Snapshotting.Model;

    class EmptyScannerResult :
        ScannerResult
    {
        public Guid Identifier => NewId.NextGuid();
        public string ScannerIdentifier => typeof(NoOpDiagnostic<EmptySnapshot>).GetIdentifier();
        public IReadOnlyList<DiagnosticAnalyzerResult> Results => new List<DiagnosticAnalyzerResult>();
        public DateTimeOffset Timestamp => DateTimeOffset.UtcNow;
    }
}