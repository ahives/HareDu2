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

    public class MessagePagingSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Memory;
        public DiagnosticSensorStatus Status => _status;

        public MessagePagingSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = DiagnosticSensorStatus.Online;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            DiagnosticResult result;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("Memory.PagedOut.Total", data.Memory.PagedOut.Total.ToString())
            };
            
            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.Memory.PagedOut.Total > 0)
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