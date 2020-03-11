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
            Console.WriteLine((string) "Probe Identifier: {0}", (object) value.Result.ProbeId);
            Console.WriteLine((string) "Probe: {0}", (object) value.Result);
            Console.WriteLine((string) "Timestamp: {0}", (object) value.Timestamp.ToString());
            Console.WriteLine((string) "Component Identifier: {0}", (object) value.Result.ComponentId);
            Console.WriteLine((string) "Component Type: {0}", (object) value.Result.ComponentType);
            Console.WriteLine((string) "Status: {0}", (object) value.Result.Status);
            Console.WriteLine((string) "Data => {0}", (object) value.Result.ProbeData.ToJsonString());

            if (value.Result.Status == DiagnosticProbeResultStatus.Unhealthy)
            {
                Console.WriteLine((string) "Reason: {0}", (object) value.Result.Article.Reason);
                Console.WriteLine((string) "Remediation: {0}", (object) value.Result.Article.Remediation);
            }

            Console.WriteLine();
        }
    }
}