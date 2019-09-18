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

    public class BlockedConnectionSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Connectivity;
        public DiagnosticSensorStatus Status => _sensorStatus;

        public BlockedConnectionSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            DiagnosticSensorResult result = new OnlineDiagnosticSensorResult(Identifier, ComponentType);

            NotifyObservers(result);

            _sensorStatus = result.Status;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("State", data.State.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.State == ConnectionState.Blocked)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.Identifier, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.Identifier, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}