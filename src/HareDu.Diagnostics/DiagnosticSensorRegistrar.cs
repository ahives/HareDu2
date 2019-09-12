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
    using Configuration;
    using KnowledgeBase;
    using Sensors;

    public class DiagnosticSensorRegistrar :
        IDiagnosticSensorRegistrar
    {
        readonly List<IDiagnosticSensor> _sensors;
        readonly List<Type> _types;

        public IReadOnlyList<IDiagnosticSensor> Sensors => _sensors;

        public DiagnosticSensorRegistrar()
        {
            _sensors = new List<IDiagnosticSensor>();
            _types = GetTypes();
        }
        
        public void RegisterAll(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            for (int i = 0; i < _types.Count; i++)
            {
                _sensors.Add((IDiagnosticSensor) Activator.CreateInstance(_types[i], configProvider, knowledgeBaseProvider));
            }
        }

        List<Type> GetTypes() =>
            typeof(IDiagnosticSensor)
                .Assembly
                .GetTypes()
                .Where(IsDiagnosticSensor)
                .ToList();

        bool IsDiagnosticSensor(Type type) => typeof(IDiagnosticSensor).IsAssignableFrom(type) && !type.IsInterface;
    }
}