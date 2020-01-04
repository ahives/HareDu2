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
namespace HareDu.AutofacIntegration
{
    using System.IO;
    using Analytics;
    using Analytics.Registration;
    using Autofac;
    using Core.Configuration;
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
                    var diagnosticAnalyzerRegistrar = x.Resolve<IDiagnosticAnalyzerRegistrar>();
                    var componentDiagnosticRegistrar = x.Resolve<IComponentDiagnosticRegistrar>();

                    return new ComponentDiagnosticFactory(diagnosticAnalyzerRegistrar, componentDiagnosticRegistrar);
                })
                .As<IComponentDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = x.Resolve<IAnalyticsRegistry>();
                    
                    registrar.RegisterAll();
                    
                    return new DiagnosticReportAnalyzerFactory(registrar.Cache);
                })
                .As<IDiagnosticReportAnalyzerFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IConfigurationProvider>();
                    string path = $"{Directory.GetCurrentDirectory()}/config.yaml";

                    configProvider.TryGet(path, out var config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();
                    
                    var registrar = new DiagnosticAnalyzerRegistrar(config.Analyzer, knowledgeBaseProvider);
                    
                    registrar.RegisterAll();

                    return registrar;
                })
                .As<IDiagnosticAnalyzerRegistrar>()
                .SingleInstance();

            builder.RegisterType<AnalyticsRegistry>()
                .As<IAnalyticsRegistry>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = new ComponentDiagnosticRegistrar(x.Resolve<IDiagnosticAnalyzerRegistrar>());
                    
                    registrar.RegisterAll();

                    return registrar;
                })
                .As<IComponentDiagnosticRegistrar>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScanner>()
                .As<IDiagnosticScanner>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
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