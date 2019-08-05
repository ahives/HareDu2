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
namespace HareDu.Internal.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alerts;
    using Core;

    public class BaseSnapshot
    {
        protected readonly IResourceFactory _factory;

        protected BaseSnapshot(IResourceFactory factory)
        {
            _factory = factory;
        }

        protected IEnumerable<IDiagnosticCheck<T>> GetDiagnosticChecks<T>()
        {
            var diagnosticChecks = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(IDiagnosticCheck<T>).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            for (int i = 0; i < diagnosticChecks.Count; i++)
            {
                yield return (IDiagnosticCheck<T>) Activator.CreateInstance(diagnosticChecks[i]);
            }
        }
    }
}