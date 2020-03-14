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
namespace HareDu.Diagnostics.Registration
{
    using System;
    using System.Collections.Generic;
    using Probes;
    using Scanners;
    using Snapshotting;

    public interface IScannerFactory
    {
        bool TryGet<T>(out DiagnosticScanner<T> scanner)
            where T : Snapshot;

        void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers);

        void RegisterObserver(IObserver<ProbeContext> observer);

        bool RegisterProbe<T>(T probe)
            where T : DiagnosticProbe;

        IReadOnlyList<string> GetAvailableProbes();
    }
}