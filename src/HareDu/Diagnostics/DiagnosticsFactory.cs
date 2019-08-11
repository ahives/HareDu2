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
    using System.Linq;
    using Core.Exceptions;
    using Reporting;

    /// <summary>
    /// This class should be instantiate once since construction can be expensive.
    /// </summary>
    public class DiagnosticsFactory :
        IDiagnosticsFactory
    {
        readonly IReadOnlyList<IDiagnostic> _diagnostics;
        readonly IDictionary<string, object> _diagnosticRunnerCache = new Dictionary<string, object>();

        public DiagnosticsFactory(IReadOnlyList<IDiagnostic> diagnostics)
        {
            _diagnostics = diagnostics;
            
            RegisterDiagnosticRunners(diagnostics);
        }

        public bool TryGet<T>(out IDiagnosticsRunner<T> runner)
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(IDiagnosticsRunner<T>).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(T)}");

            if (_diagnosticRunnerCache.ContainsKey(type.FullName))
            {
                runner = (IDiagnosticsRunner<T>) _diagnosticRunnerCache[type.FullName];
                return true;
            }

            try
            {
                runner = Activator.CreateInstance(type, _diagnostics) as IDiagnosticsRunner<T>;

                // TODO: check to see if instance was created successfully
            
                _diagnosticRunnerCache.Add(type.FullName, runner);
                return true;
            }
            catch (Exception e)
            {
            }
            
            runner = new FaultedDiagnosticsRunner<T>();
            return false;
        }

        void RegisterDiagnosticRunners(IReadOnlyList<IDiagnostic> diagnostics)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(IDiagnosticsRunner<>).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, diagnostics);
                
                _diagnosticRunnerCache.Add(type.FullName, instance);
            }
        }
    }
}