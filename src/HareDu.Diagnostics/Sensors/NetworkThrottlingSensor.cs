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

    public class NetworkThrottlingSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();
        public string Description =>
            "Checks network to see if the number of sockets currently in use is less than or equal to the number available.";
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;

        public NetworkThrottlingSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _canReadConfig = _configProvider.TryGet(out _config);
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            
            if (!_canReadConfig)
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            NodeSnapshot data = snapshot as NodeSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.OS.Sockets.Available);
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("OS.Sockets.Available", data.OS.Sockets.Available.ToString()),
                new DiagnosticSensorDataImpl("OS.Sockets.Used", data.OS.Sockets.Used.ToString()),
                new DiagnosticSensorDataImpl("CalculatedWarningThreshold", warningThreshold.ToString())
            };

            if (data.OS.Sockets.Used < warningThreshold && warningThreshold < data.OS.Sockets.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else if (data.OS.Sockets.Used == data.OS.Sockets.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }

        ulong ComputeWarningThreshold(ulong socketsAvailable)
            => _config.Sensor.SocketUsageCoefficient >= 1
                ? socketsAvailable
                : Convert.ToUInt64(Math.Ceiling(socketsAvailable * _config.Sensor.SocketUsageCoefficient));
    }
}