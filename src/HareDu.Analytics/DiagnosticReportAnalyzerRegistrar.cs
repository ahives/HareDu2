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
namespace HareDu.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiagnosticReportAnalyzerRegistrar :
        IDiagnosticReportAnalyzerRegistrar
    {
        readonly List<Type> _types;
        readonly List<IDiagnosticReportAnalyzer> _analyzers;

        public IReadOnlyList<IDiagnosticReportAnalyzer> Analyzers => _analyzers;
        
        public DiagnosticReportAnalyzerRegistrar()
        {
            _analyzers = new List<IDiagnosticReportAnalyzer>();
            _types = GetTypes();
        }

        public void RegisterAll()
        {
            foreach (var type in _types)
            {
                RegisterInstance(type);
            }
        }

        public void Register(Type type)
        {
            _types.Add(type);
            
            RegisterInstance(type);
        }

        void RegisterInstance(Type type)
        {
            try
            {
                var instance = (IDiagnosticReportAnalyzer)Activator.CreateInstance(type);
            
                _analyzers.Add(instance);
            }
            catch { }
        }

        List<Type> GetTypes() =>
            GetType()
                .Assembly
                .GetTypes()
                .Where(IsDiagnosticReportAnalyzer)
                .ToList();

        bool IsDiagnosticReportAnalyzer(Type type)
            => type.IsClass
               && !type.IsGenericType
               && type.GetInterfaces().Any(x => x == typeof(IDiagnosticReportAnalyzer));
    }
}