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
    using Analytics;
    using Analytics.Registration;
    using Autofac;
    using Diagnostics.Configuration;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Scanning;

    public class HareDuAnalyticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IDiagnosticScannerConfigProvider>();
                    
                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();

                    var analyzerRegistrar = x.Resolve<IDiagnosticAnalyzerRegistration>();
                    analyzerRegistrar.RegisterAll(configProvider, knowledgeBaseProvider);

                    var diagnosticsRegistrar = x.Resolve<IComponentDiagnosticRegistration>();
                    diagnosticsRegistrar.RegisterAll(analyzerRegistrar.Analyzers);

                    return new ComponentDiagnosticFactory(diagnosticsRegistrar.Cache, diagnosticsRegistrar.Types, analyzerRegistrar.Analyzers);
                })
                .As<IComponentDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = x.Resolve<IAnalyticsRegistration>();
                    
                    registrar.RegisterAll();
                    
                    return new DiagnosticReportAnalyzerFactory(registrar.Cache);
                })
                .As<IDiagnosticReportAnalyzerFactory>()
                .SingleInstance();

            builder.RegisterType<AnalyticsRegistration>()
                .As<IAnalyticsRegistration>()
                .SingleInstance();

            builder.RegisterType<ComponentDiagnosticRegistration>()
                .As<IComponentDiagnosticRegistration>()
                .SingleInstance();

            builder.RegisterType<DiagnosticAnalyzerRegistration>()
                .As<IDiagnosticAnalyzerRegistration>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScanner>()
                .As<IDiagnosticScanner>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScannerConfigProvider>()
                .As<IDiagnosticScannerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<DefaultKnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}