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

    class HighConnectionCreationRateSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName;
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Connectivity;

        public HighConnectionCreationRateSensor(IDiagnosticSensorConfigProvider provider)
            : base(provider)
        {
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            ConnectivitySnapshot data = snapshot as ConnectivitySnapshot;
            DiagnosticResult result;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }
            
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("ConnectionsCreated.Rate", data.ConnectionsCreated.Rate.ToString())
            };

            if (!_provider.TryGet(out DiagnosticSensorConfig config))
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType, sensorData);

                NotifyObservers(result);

                return result;
            }

            sensorData.Add(new DiagnosticSensorDataImpl("RateThreshold", config.Connection.HighCreationRateThreshold.ToString()));
            
            result = data.ConnectionsCreated.Rate >= config.Connection.HighCreationRateThreshold
                ? new WarningDiagnosticResult(null, Identifier, ComponentType, sensorData, null, null)
                : new PositiveDiagnosticResult(null, Identifier, ComponentType, sensorData, null) as DiagnosticResult;

            NotifyObservers(result);

            return result;
        }
    }
}