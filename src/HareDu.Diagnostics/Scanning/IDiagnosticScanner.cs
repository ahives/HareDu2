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
    using Snapshotting;

    public interface IDiagnosticScanner
    {
        /// <summary>
        /// Executes a list of predefined diagnostic sensors against the snapshot and returns a concatenated report of the results from executing said sensors.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DiagnosticReport Scan<T>(T snapshot)
            where T : Snapshot;
        
        /// <summary>
        /// Registers a list of observers that receives the output in real-time as the each sensor executes.
        /// </summary>
        /// <param name="observers"></param>
        IDiagnosticScanner RegisterObservers(IReadOnlyList<IObserver<DiagnosticContext>> observers);

        /// <summary>
        /// Registers an observer that receives the output in real-time as the each sensor executes.
        /// </summary>
        /// <param name="observer"></param>
        IDiagnosticScanner RegisterObserver(IObserver<DiagnosticContext> observer);
    }
}