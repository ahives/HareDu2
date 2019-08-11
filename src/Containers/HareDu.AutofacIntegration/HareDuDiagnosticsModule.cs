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
namespace HareDu.AutofacIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Diagnostics;

    public class HareDuDiagnosticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var diagnostics = RegisterDiagnostics().ToList();

                    return new DiagnosticsFactory(diagnostics);
                })
                .As<IDiagnosticsFactory>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportGenerator>()
                .As<IGenerateDiagnosticReport>()
                .SingleInstance();

            base.Load(builder);
        }
        
        IEnumerable<IDiagnostic> RegisterDiagnostics()
        {
            var diagnostics = typeof(IDiagnostic)
                .Assembly
                .GetTypes()
                .Where(x => typeof(IDiagnostic).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            for (int i = 0; i < diagnostics.Count; i++)
            {
                yield return (IDiagnostic) Activator.CreateInstance(diagnostics[i]);
            }
        }
    }
}