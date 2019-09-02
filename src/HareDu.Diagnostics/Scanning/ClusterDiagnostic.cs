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

    public class ClusterDiagnostic :
        IComponentDiagnostic<ClusterSnapshot>
    {
        readonly IEnumerable<IDiagnosticSensor> _nodeSensors;
        readonly IEnumerable<IDiagnosticSensor> _diskSensors;
        readonly IEnumerable<IDiagnosticSensor> _memorySensors;
        readonly IEnumerable<IDiagnosticSensor> _runtimeSensors;

        public ClusterDiagnostic(IReadOnlyList<IDiagnosticSensor> sensors)
        {
            _nodeSensors = sensors.Where(IsNodeSensor);
            _diskSensors = sensors.Where(IsDiskSensor);
            _memorySensors = sensors.Where(IsMemorySensor);
            _runtimeSensors = sensors.Where(IsRuntimeSensor);
        }

        public IReadOnlyList<DiagnosticResult> Scan(ClusterSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();

            for (int i = 0; i < snapshot.Nodes.Count; i++)
            {
                results.AddRange(_nodeSensors.Select(x => x.Execute(snapshot.Nodes[i])));
                results.AddRange(_diskSensors.Select(x => x.Execute(snapshot.Nodes[i].Disk)));
                results.AddRange(_memorySensors.Select(x => x.Execute(snapshot.Nodes[i].Memory)));
                results.AddRange(_runtimeSensors.Select(x => x.Execute(snapshot.Nodes[i].Runtime)));
            }

            return results;
        }

        bool IsRuntimeSensor(IDiagnosticSensor sensor) => !sensor.IsNull() && sensor.ComponentType == ComponentType.Runtime;

        bool IsMemorySensor(IDiagnosticSensor sensor) => !sensor.IsNull() && sensor.ComponentType == ComponentType.Memory;

        bool IsDiskSensor(IDiagnosticSensor sensor) => !sensor.IsNull() && sensor.ComponentType == ComponentType.Disk;

        bool IsNodeSensor(IDiagnosticSensor sensor) => !sensor.IsNull() && sensor.ComponentType == ComponentType.Node;
    }
}