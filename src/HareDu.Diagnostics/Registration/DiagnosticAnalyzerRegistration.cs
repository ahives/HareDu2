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
namespace HareDu.Diagnostics.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Analyzers;
    using Core.Configuration;
    using KnowledgeBase;

    public class DiagnosticAnalyzerRegistration :
        IDiagnosticAnalyzerRegistration
    {
        readonly List<IDiagnosticAnalyzer> _analyzers;
        readonly List<Type> _types;

        public IReadOnlyList<IDiagnosticAnalyzer> Analyzers => _analyzers;

        public DiagnosticAnalyzerRegistration()
        {
            _analyzers = new List<IDiagnosticAnalyzer>();
            _types = GetTypes();
        }
        
        public void RegisterAll(string path, IConfigurationProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            configProvider.TryGet(path, out HareDuConfig config);
            
            for (int i = 0; i < _types.Count; i++)
            {
                _analyzers.Add((IDiagnosticAnalyzer) Activator.CreateInstance(_types[i], config.Analyzer, knowledgeBaseProvider));
            }
        }

        List<Type> GetTypes() =>
            typeof(IDiagnosticAnalyzer)
                .Assembly
                .GetTypes()
                .Where(IsDiagnosticSensor)
                .ToList();

        bool IsDiagnosticSensor(Type type) => typeof(IDiagnosticAnalyzer).IsAssignableFrom(type) && !type.IsInterface;
    }
}