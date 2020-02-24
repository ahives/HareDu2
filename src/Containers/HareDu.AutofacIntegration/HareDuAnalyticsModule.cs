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
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;

    public class HareDuAnalyticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IFileConfigProvider>();
                    string path = $"{Directory.GetCurrentDirectory()}/haredu.yaml";

                    configProvider.TryGet(path, out var config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();

                    return new DiagnosticFactory(config.Diagnostics, knowledgeBaseProvider);
                })
                .As<IDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registrar = x.Resolve<IAnalyticsRegistry>();
                    
                    registrar.RegisterAll();
                    
                    return new DiagnosticReportAnalyzerFactory(registrar.Cache);
                })
                .As<IDiagnosticReportAnalyzerFactory>()
                .SingleInstance();

            builder.RegisterType<AnalyticsRegistry>()
                .As<IAnalyticsRegistry>()
                .SingleInstance();

            builder.RegisterType<DiagnosticScanner>()
                .As<IDiagnosticScanner>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}