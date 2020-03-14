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
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using KnowledgeBase;
    using Registration;
    using Scans;
    using Snapshotting;

    public class DiagnosticScanner :
        IDiagnosticScanner
    {
        readonly DiagnosticsConfig _config;
        readonly IScannerFactory _factory;

        public DiagnosticScanner(IScannerFactory factory)
        {
            _factory = factory;
        }

        public DiagnosticScanner(DiagnosticsConfig config)
        {
            _config = config;
            _factory = new ScannerFactory(_config, new KnowledgeBaseProvider());
        }

        public DiagnosticScanner(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
        {
            _config = config;
            _factory = new ScannerFactory(_config, kb);
        }

        public ScannerResult Scan<T>(T snapshot)
            where T : Snapshot
        {
            if (!_factory.TryGet(out DiagnosticScan<T> scanner))
                return DiagnosticCache.EmptyScannerResult;
            
            var results = scanner.Scan(snapshot);
            
            return new SuccessfulScannerResult(scanner.Identifier, results);
        }

        public IDiagnosticScanner RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers)
        {
            _factory.RegisterObservers(observers);

            return this;
        }

        public IDiagnosticScanner RegisterObserver(IObserver<ProbeContext> observer)
        {
            _factory.RegisterObserver(observer);

            return this;
        }
    }
}