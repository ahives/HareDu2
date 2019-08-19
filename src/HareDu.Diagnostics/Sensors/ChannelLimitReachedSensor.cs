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

    class ChannelLimitReachedSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => GetType().FullName;
        public string Description => "Measures actual number of channels to the defined limit on connection";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;

        public ChannelLimitReachedSensor(IDiagnosticSensorConfigProvider provider)
            : base(provider)
        {
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;
            DiagnosticResult result;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("Channels.Count", data.Channels.Count.ToString()),
                new DiagnosticSensorDataImpl("ChannelLimit", data.ChannelLimit.ToString())
            };

            result = data.Channels.Count >= data.ChannelLimit
                ? new NegativeDiagnosticResult(data.Identifier, Identifier, ComponentType, sensorData,
                    "Number of channels on connection exceeds the defined limit.",
                    "Adjust application settings to reduce the number of connections to the RabbitMQ broker.")
                : new PositiveDiagnosticResult(data.Identifier, Identifier, ComponentType, sensorData,
                    "Number of channels on connection is less than defined limit.") as DiagnosticResult;

            NotifyObservers(result);

            return result;
        }
    }
}