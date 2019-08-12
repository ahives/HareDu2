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
namespace HareDu.Diagnostics.Scanning
{
    using System;
    using System.Collections.Generic;
    using Snapshotting.Model;

    public class DiagnosticScanner :
        IDiagnosticScanner
    {
        readonly IComponentDiagnosticFactory _factory;

        public DiagnosticScanner(IComponentDiagnosticFactory factory)
        {
            _factory = factory;
        }

        public DiagnosticReport Scan<T>(T snapshot)
            where T : Snapshot
        {
            if (!_factory.TryGet(out IComponentDiagnostic<T> diagnostic))
                return new FaultedDiagnosticReport();
            
            var results = diagnostic.Scan(snapshot);
            
            return new SuccessfulDiagnosticReport(results);
        }

        public void RegisterObservers(IList<IObserver<DiagnosticContext>> observers)
        {
            _factory.RegisterObservers(observers);
        }

        public void RegisterObserver(IObserver<DiagnosticContext> observer)
        {
            _factory.RegisterObserver(observer);
        }
    }
}