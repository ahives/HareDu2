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
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class RedeliveredMessagesSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.FaultTolerance;
        public DiagnosticSensorStatus Status => _sensorStatus;

        public RedeliveredMessagesSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
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
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString()),
                new DiagnosticSensorDataImpl("Messages.Redelivered.Total", data.Messages.Redelivered.Total.ToString()),
                new DiagnosticSensorDataImpl("MessageRedeliveryCoefficient", _config.Sensor.MessageRedeliveryCoefficient.ToString()),
                new DiagnosticSensorDataImpl("MessageRedeliveryCoefficient", _config.Sensor.MessageRedeliveryCoefficient.ToString())
            };
            
            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.Messages.Incoming.Total);
            
            if (data.Messages.Redelivered.Total >= warningThreshold && data.Messages.Redelivered.Total < data.Messages.Incoming.Total && warningThreshold < data.Messages.Incoming.Total)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else if (data.Messages.Redelivered.Total >= data.Messages.Incoming.Total)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong total)
            => _config.Sensor.MessageRedeliveryCoefficient >= 1
                ? total
                : Convert.ToUInt64(Math.Ceiling(total * _config.Sensor.MessageRedeliveryCoefficient));
    }
}