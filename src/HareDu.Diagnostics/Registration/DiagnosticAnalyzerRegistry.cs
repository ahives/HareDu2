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

    public class DiagnosticAnalyzerRegistry :
        IDiagnosticAnalyzerRegistry
    {
        readonly DiagnosticAnalyzerConfig _config;
        readonly IKnowledgeBaseProvider _knowledgeBaseProvider;
        readonly IConfigurationProvider _configProvider;
        readonly List<IDiagnosticAnalyzer> _cache;
        readonly List<Type> _types;

        public IReadOnlyList<IDiagnosticAnalyzer> ObjectCache => _cache;

        public DiagnosticAnalyzerRegistry(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            _config = config;
            _knowledgeBaseProvider = knowledgeBaseProvider;
            _cache = new List<IDiagnosticAnalyzer>();
            _types = GetTypes();
        }
        
        public void RegisterAll()
        {
            _cache.Clear();
            
            for (int i = 0; i < _types.Count; i++)
            {
                _cache.Add((IDiagnosticAnalyzer) Activator.CreateInstance(_types[i], _config, _knowledgeBaseProvider));
            }
        }

        public void Register(Type type)
        {
            if (_cache.Any(x => x.Identifier == type.GetIdentifier()))
                return;
            
            try
            {
                var instance = (IDiagnosticAnalyzer)Activator.CreateInstance(type, _config, _knowledgeBaseProvider);
            
                _cache.Add(instance);
            }
            catch { }
        }

        public void Register<T>()
        {
            Type type = typeof(T);
            if (_cache.Any(x => x.Identifier == type.GetIdentifier()))
                return;
            
            try
            {
                var instance = (IDiagnosticAnalyzer)Activator.CreateInstance(type, _config, _knowledgeBaseProvider);
            
                _cache.Add(instance);
            }
            catch { }
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