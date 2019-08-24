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
    using Snapshotting.Model;

    class QueueMessagePagingSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName;
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Memory;

        public QueueMessagePagingSensor(IDiagnosticSensorConfigProvider provider)
            : base(provider)
        {
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
                new DiagnosticSensorDataImpl("Memory.RAM.Total", data.Memory.RAM.Total.ToString())
            };
            
            result = data.Memory.RAM.Total > 0
                ? new WarningDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, null, null)
                : new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, null) as DiagnosticResult;

            NotifyObservers(result);

            return result;
        }
    }
}