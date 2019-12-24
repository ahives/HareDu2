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
    using System.IO;
    using Autofac;
    using Core;
    using Core.Configuration;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Scanning;
    using Registration;
    using Snapshotting;
    using Snapshotting.Registration;

    public class HareDuDiagnosticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var analyzerRegistry = x.Resolve<IDiagnosticAnalyzerRegistry>();
                    
                    analyzerRegistry.RegisterAll();

                    var diagnosticRegistry = x.Resolve<IComponentDiagnosticRegistry>();
                    
                    diagnosticRegistry.RegisterAll();

                    return new ComponentDiagnosticFactory(diagnosticRegistry.ObjectCache, diagnosticRegistry.Types, analyzerRegistry.ObjectCache);
                })
                .As<IComponentDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IConfigurationProvider>();
                    string path = $"{Directory.GetCurrentDirectory()}/config.yaml";

                    configProvider.TryGet(path, out HareDuConfig config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();
                    
                    return new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
                })
                .As<IDiagnosticAnalyzerRegistry>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var brokerObjectRegistry = x.Resolve<IBrokerObjectRegistry>();
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var brokerConnection = x.Resolve<IBrokerConnectionClient>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                    
                    var client = brokerConnection.Create(settings);

                    brokerObjectRegistry.RegisterAll(client);

                    return new BrokerObjectFactory(client, brokerObjectRegistry.ObjectCache);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var snapshotObjectRegistry = x.Resolve<ISnapshotObjectRegistry>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    snapshotObjectRegistry.RegisterAll(factory);

                    return new SnapshotFactory(factory, snapshotObjectRegistry.ObjectCache);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var analyzerRegistry = x.Resolve<IDiagnosticAnalyzerRegistry>();

                    analyzerRegistry.RegisterAll();

                    return new ComponentDiagnosticRegistry(analyzerRegistry.ObjectCache);
                })
                .As<IComponentDiagnosticRegistry>()
                .SingleInstance();

            builder.RegisterType<SnapshotObjectRegistry>()
                .As<ISnapshotObjectRegistry>()
                .SingleInstance();

            builder.RegisterType<BrokerObjectRegistry>()
                .As<IBrokerObjectRegistry>()
                .SingleInstance();

            builder.RegisterType<BrokerConnectionClient>()
                .As<IBrokerConnectionClient>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
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