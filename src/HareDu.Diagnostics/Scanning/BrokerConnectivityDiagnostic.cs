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
namespace HareDu.Diagnostics.Scanning
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Sensors;
    using Snapshotting.Model;

    public class BrokerConnectivityDiagnostic :
        IComponentDiagnostic<BrokerConnectivitySnapshot>
    {
        public string Identifier => GetType().FullName.GenerateIdentifier();

        readonly IReadOnlyList<IDiagnosticSensor> _channelSensors;
        readonly IReadOnlyList<IDiagnosticSensor> _connectionSensors;
        readonly IReadOnlyList<IDiagnosticSensor> _connectivitySensors;

        public BrokerConnectivityDiagnostic(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _connectionSensors = sensors.Where(IsConnectionThroughputSensor).ToList();
            _channelSensors = sensors.Where(IsChannelThroughputSensor).ToList();
            _connectivitySensors = sensors.Where(IsConnectivitySensor).ToList();
        }

        public IReadOnlyList<DiagnosticResult> Scan(BrokerConnectivitySnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();
            
            results.AddRange(_connectivitySensors.Select(x => x.Execute(snapshot)));
            
            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                results.AddRange(_connectionSensors.Select(x => x.Execute(snapshot.Connections[i])));

                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    results.AddRange(_channelSensors.Select(x => x.Execute(snapshot.Connections[i].Channels[j])));
                }
            }

            return results;
        }

        bool IsChannelThroughputSensor(IDiagnosticSensor sensor) =>
            !sensor.IsNull()
            && sensor.ComponentType == ComponentType.Channel
            && sensor.SensorCategory != DiagnosticSensorCategory.Connectivity;

        bool IsConnectionThroughputSensor(IDiagnosticSensor sensor) =>
            !sensor.IsNull()
            && sensor.ComponentType == ComponentType.Connection
            && sensor.SensorCategory != DiagnosticSensorCategory.Connectivity;

        bool IsConnectivitySensor(IDiagnosticSensor sensor) =>
            !sensor.IsNull()
            && (sensor.ComponentType == ComponentType.Connection || sensor.ComponentType == ComponentType.Channel)
            && sensor.SensorCategory == DiagnosticSensorCategory.Connectivity;
    }
}