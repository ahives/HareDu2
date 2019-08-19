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
namespace HareDu.Diagnostics.Observers
{
    using System;
    using Core.Extensions;

    public class DefaultDiagnosticConsoleLogger :
        IObserver<DiagnosticContext>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DiagnosticContext value)
        {
            Console.WriteLine("Timestamp: {0}", value.Timestamp.ToString());
            Console.WriteLine("Component Identifier: {0}", value.Result.ComponentIdentifier);
            Console.WriteLine("Component Type: {0}", value.Result.ComponentType);
            Console.WriteLine("Sensor: {0}", value.Result.SensorIdentifier);
            Console.WriteLine("Status: {0}", value.Result.Status);
            Console.WriteLine("Data => {0}", value.Result.SensorData.ToJsonString());

            if (value.Result.Status == DiagnosticStatus.Red)
            {
                Console.WriteLine("Reason: {0}", value.Result.Reason);
                Console.WriteLine("Remediation: {0}", value.Result.Remediation);
            }

            Console.WriteLine();
        }
    }
}