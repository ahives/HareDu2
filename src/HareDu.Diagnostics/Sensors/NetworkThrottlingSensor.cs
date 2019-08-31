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
        public string Identifier => GetType().FullName.ComputeHash();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;

        public NetworkThrottlingSensor(IDiagnosticSensorConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
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
            long highWatermark = CalculateHighWatermark(data.OS.Sockets.Available);
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("OS.Sockets.Available", data.OS.Sockets.Available.ToString()),
                new DiagnosticSensorDataImpl("OS.Sockets.Used", data.OS.Sockets.Used.ToString()),
                new DiagnosticSensorDataImpl("CalculatedHighWatermark", highWatermark.ToString())
            };

//            if (highWatermark < data.OS.Sockets.Available)
//            {
//                if (data.OS.Sockets.Used >= highWatermark && data.OS.Sockets.Used < data.OS.Sockets.Available)
//                {
//                    _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
//                    result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
//                }
//            }
//            if (data.OS.Sockets.Used >= highWatermark && highWatermark < data.OS.Sockets.Available)
//            {
//                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
//                result = new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
//            }
//            else if (data.OS.Sockets.Used == highWatermark && highWatermark == data.OS.Sockets.Available)
//            {
//                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
//                result = new NegativeDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
//            }
//            else
//            {
//                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
//                result = new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
//            }

            if (data.OS.Sockets.Used < highWatermark)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else if (data.OS.Sockets.Used >= highWatermark && highWatermark >= data.OS.Sockets.Available)
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

        long CalculateHighWatermark(long socketsAvailable)
        {
            if (_config.Node.SocketUsageCoefficient == 1.0M)
                return socketsAvailable;
            
            long highWatermark = Convert.ToInt64(Math.Ceiling(socketsAvailable * _config.Node.SocketUsageCoefficient));

            return highWatermark;
        }
    }
}