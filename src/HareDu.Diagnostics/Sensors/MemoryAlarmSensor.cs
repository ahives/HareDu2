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
namespace HareDu.Diagnostics.Sensors
{
    using System.Collections.Generic;
    using Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class MemoryAlarmSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Memory;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;
        public DiagnosticSensorStatus Status => _status;

        public MemoryAlarmSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = DiagnosticSensorStatus.Online;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            MemorySnapshot data = snapshot as MemorySnapshot;
            DiagnosticResult result;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("Memory.FreeAlarm", data.AlarmInEffect.ToString()),
                new DiagnosticSensorDataImpl("Memory.Limit", data.Limit.ToString()),
                new DiagnosticSensorDataImpl("Memory.Used", data.Used.ToString())
            };

            if (data.AlarmInEffect)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(null, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(null, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}