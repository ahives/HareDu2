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
    using Internal;
    using Snapshotting.Model;

    class ChannelThrottlingSensor :
        BaseDiagnosticSensor,
        IDiagnosticSensor
    {
        public string Identifier => "ChannelThrottling";
        public string Description => "Monitors connections to the RabbitMQ broker to determine whether channels are being throttled.";
        public ComponentType ComponentType => ComponentType.Channel;
        public DiagnosticSensorCategory SensorCategory => DiagnosticSensorCategory.Throughput;

        public DiagnosticResult Execute<T>(T snapshot)
        {
            ChannelSnapshot data = snapshot as ChannelSnapshot;
            var sensorData = new List<DiagnosticSensorData>
            {
                new DiagnosticSensorDataImpl("UnacknowledgedMessages", data.UnacknowledgedMessages.ToString()),
                new DiagnosticSensorDataImpl("PrefetchCount", data.PrefetchCount.ToString())
            };

            DiagnosticResult result = data.UnacknowledgedMessages > data.PrefetchCount
                ? new NegativeDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, DiagnosticStatus.Red,
                    "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to stop delivering messages to consumers.",
                    "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1. Temporarily increase the number of consumers or prefetch count.")
                : new PositiveDiagnosticResult(data.Name, Identifier, ComponentType, sensorData, DiagnosticStatus.Green,
                    "Unacknowledged messages on channel is less than prefetch count.") as DiagnosticResult;

            NotifyObservers(result);
                
            return result;
        }
    }
}