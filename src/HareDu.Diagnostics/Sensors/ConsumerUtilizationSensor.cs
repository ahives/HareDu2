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

    public class ConsumerUtilizationSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;
        public DiagnosticSensorStatus Status => _sensorStatus;

        public ConsumerUtilizationSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            DiagnosticSensorResult result = _configProvider.TryGet(out _config)
                ? (DiagnosticSensorResult) new OnlineDiagnosticSensorResult(Identifier, ComponentType)
                : new OfflineDiagnosticSensorResult(Identifier, ComponentType);

            NotifyObservers(result);

            _sensorStatus = result.Status;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("ConsumerUtilization", data.ConsumerUtilization.ToString()),
                new DiagnosticSensorDataImpl("Sensor.ConsumerUtilizationWarningCoefficient", _config.Sensor.ConsumerUtilizationWarningCoefficient.ToString())
            };

            if (data.ConsumerUtilization >= _config.Sensor.ConsumerUtilizationWarningCoefficient && data.ConsumerUtilization < 1.0M)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else if (data.ConsumerUtilization < _config.Sensor.ConsumerUtilizationWarningCoefficient)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}