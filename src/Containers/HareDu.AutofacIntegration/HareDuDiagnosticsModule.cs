﻿// Copyright 2013-2019 Albert L. Hives
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
    using Diagnostics.Scanning;
    using Diagnostics.Sensors;

    public class HareDuDiagnosticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var sensors = RegisterDiagnosticSensors().ToList();

                    return new ComponentDiagnosticFactory(sensors);
                })
                .As<IComponentDiagnosticFactory>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScanner>()
                .As<IDiagnosticScanner>()
                .SingleInstance();

            base.Load(builder);
        }
        
        IEnumerable<IDiagnosticSensor> RegisterDiagnosticSensors()
        {
            var diagnostics = typeof(IDiagnosticSensor)
                .Assembly
                .GetTypes()
                .Where(x => typeof(IDiagnosticSensor).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            for (int i = 0; i < diagnostics.Count; i++)
            {
                yield return (IDiagnosticSensor) Activator.CreateInstance(diagnostics[i]);
            }
        }
    }
}