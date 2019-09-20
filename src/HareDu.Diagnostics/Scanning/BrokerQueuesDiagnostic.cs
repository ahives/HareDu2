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

    public class BrokerQueuesDiagnostic :
        IComponentDiagnostic<BrokerQueuesSnapshot>
    {
        public string Identifier => GetType().GenerateIdentifier();

        readonly IReadOnlyList<IDiagnosticSensor> _queueSensors;
        readonly List<IDiagnosticSensor> _exchangeSensors;

        public BrokerQueuesDiagnostic(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _queueSensors = sensors.Where(IsQueueSensor).ToList();
            _exchangeSensors = sensors.Where(IsExchangeSensor).ToList();
        }

        public IReadOnlyList<DiagnosticResult> Scan(BrokerQueuesSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();

            results.AddRange(_exchangeSensors.Select(x => x.Execute(snapshot)));
            
            for (int i = 0; i < snapshot.Queues.Count; i++)
            {
                results.AddRange(_queueSensors.Select(x => x.Execute(snapshot.Queues[i])));
            }

            return results;
        }

        bool IsExchangeSensor(IDiagnosticSensor sensor) =>
            !sensor.IsNull()
            && sensor.Status == DiagnosticSensorStatus.Online
            && sensor.ComponentType == ComponentType.Exchange;

        bool IsQueueSensor(IDiagnosticSensor sensor) =>
            !sensor.IsNull()
            && sensor.Status == DiagnosticSensorStatus.Online
            && sensor.ComponentType == ComponentType.Queue;
    }
}