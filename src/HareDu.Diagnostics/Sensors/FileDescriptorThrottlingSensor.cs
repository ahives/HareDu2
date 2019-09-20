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

    public class FileDescriptorThrottlingSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().GenerateIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;
        public DiagnosticSensorStatus Status => _status;

        public FileDescriptorThrottlingSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = _configProvider.TryGet(out _config) ? DiagnosticSensorStatus.Online : DiagnosticSensorStatus.Offline;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            OperatingSystemSnapshot data = snapshot as OperatingSystemSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.FileDescriptors.Available);

            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("FileDescriptors.Available", data.FileDescriptors.Available.ToString()),
                new DiagnosticSensorDataImpl("FileDescriptors.Used", data.FileDescriptors.Used.ToString()),
                new DiagnosticSensorDataImpl("FileDescriptorUsageWarningThreshold", _config.Sensor.FileDescriptorUsageWarningCoefficient.ToString()),
                new DiagnosticSensorDataImpl("CalculatedWarningThreshold", warningThreshold.ToString())
            };

            if (data.FileDescriptors.Used < warningThreshold && warningThreshold < data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.ProcessId, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else if (data.FileDescriptors.Used == data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.ProcessId, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.ProcessId, Identifier, ComponentType, sensorData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong fileDescriptorsAvailable)
            => _config.Sensor.FileDescriptorUsageWarningCoefficient >= 1
                ? fileDescriptorsAvailable
                : Convert.ToUInt64(Math.Ceiling(fileDescriptorsAvailable * _config.Sensor.FileDescriptorUsageWarningCoefficient));
    }
}