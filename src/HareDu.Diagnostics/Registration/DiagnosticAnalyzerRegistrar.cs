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
    using System.Linq;
    using Analyzers;
    using Core.Configuration;
    using KnowledgeBase;

    public class DiagnosticAnalyzerRegistrar :
        IDiagnosticAnalyzerRegistrar
    {
        readonly DiagnosticAnalyzerConfig _config;
        readonly IKnowledgeBaseProvider _knowledgeBaseProvider;
        readonly IConfigurationProvider _configProvider;
        readonly IDictionary<string, IDiagnosticAnalyzer> _cache;
        readonly List<Type> _types;

        public IDictionary<string, IDiagnosticAnalyzer> ObjectCache => _cache;

        public DiagnosticAnalyzerRegistrar(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            _config = config;
            _knowledgeBaseProvider = knowledgeBaseProvider;
            _cache = new Dictionary<string, IDiagnosticAnalyzer>();
            _types = GetTypes();
        }
        
        public void RegisterAll()
        {
            for (int i = 0; i < _types.Count; i++)
            {
                if (_cache.ContainsKey(_types[i].GetIdentifier()))
                    continue;
                
                RegisterInstance(_types[i]);
            }
        }

        public void Register(Type type)
        {
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;
            
            RegisterInstance(type);
        }

        public void Register<T>()
        {
            Type type = typeof(T);
            if (_cache.ContainsKey(type.GetIdentifier()))
                return;

            RegisterInstance(type);
        }

        void RegisterInstance(Type type)
        {
            try
            {
                var instance = (IDiagnosticAnalyzer)Activator.CreateInstance(type, _config, _knowledgeBaseProvider);
            
                _cache.Add(type.GetIdentifier(), instance);
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