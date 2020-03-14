// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.IntegrationTesting.Diagnostics.Observers
{
    using System;
    using Core.Extensions;
    using HareDu.Diagnostics;

    public class DefaultDiagnosticConsoleLogger :
        IObserver<ProbeContext>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(ProbeContext value)
        {
            Console.WriteLine("Probe Id: {0}", value.Result.Id);
            Console.WriteLine("Probe: {0}", value.Result);
            Console.WriteLine("Timestamp: {0}", value.Timestamp.ToString());
            Console.WriteLine("Component Id: {0}", value.Result.ComponentId);
            Console.WriteLine("Component Type: {0}", value.Result.ComponentType);
            Console.WriteLine("Status: {0}", value.Result.Status);
            Console.WriteLine("Data => {0}", value.Result.Data.ToJsonString());

            if (value.Result.Status == ProbeResultStatus.Unhealthy)
            {
                Console.WriteLine("Reason: {0}", value.Result.KB.Reason);
                Console.WriteLine("Remediation: {0}", value.Result.KB.Remediation);
            }

            Console.WriteLine();
        }
    }
}