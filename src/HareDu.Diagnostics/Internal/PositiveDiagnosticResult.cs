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
namespace HareDu.Diagnostics.Internal
{
    using System;
    using System.Collections.Generic;

    class PositiveDiagnosticResult :
        DiagnosticResult
    {
        public PositiveDiagnosticResult(string componentIdentifier, string sensorIdentifier,
            ComponentType componentType, List<DiagnosticSensorData> sensorData, string reason)
        {
            ComponentIdentifier = componentIdentifier;
            SensorIdentifier = sensorIdentifier;
            ComponentType = componentType;
            SensorData = sensorData;
            Status = DiagnosticStatus.Green;
            Reason = reason;
            Timestamp = DateTimeOffset.Now;
        }

        public string ComponentIdentifier { get; }
        public ComponentType ComponentType { get; }
        public string SensorIdentifier { get; }
        public DiagnosticStatus Status { get; }
        public string Reason { get; }
        public string Remediation { get; }
        public IReadOnlyList<DiagnosticSensorData> SensorData { get; }
        public DateTimeOffset Timestamp { get; }
    }
}