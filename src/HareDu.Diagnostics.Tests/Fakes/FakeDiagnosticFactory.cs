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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using Diagnostics.Probes;
    using Diagnostics.Registration;
    using Scans;
    using Snapshotting;

    public class FakeDiagnosticFactory :
        IDiagnosticFactory
    {
        public bool TryGet<T>(out DiagnosticScan<T> diagnosticScan)
            where T : Snapshot
        {
            diagnosticScan = new NoOpDiagnosticScan<T>();
            return false;
        }

        public void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers)
        {
            throw new NotImplementedException();
        }

        public void RegisterObserver(IObserver<ProbeContext> observer)
        {
            throw new NotImplementedException();
        }

        public bool RegisterProbe<T>(T probe) where T : DiagnosticProbe => throw new NotImplementedException();

        public IReadOnlyList<string> GetAvailableProbes() => throw new NotImplementedException();
    }
}