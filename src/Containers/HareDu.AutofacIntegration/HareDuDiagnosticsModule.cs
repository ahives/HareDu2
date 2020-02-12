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
    using Autofac;
    using Core;
    using Core.Configuration;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Scanning;
    using Registration;
    using Snapshotting.Registration;

    public class HareDuDiagnosticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var configProvider = x.Resolve<IConfigurationProvider>();
                    string path = $"{Directory.GetCurrentDirectory()}/config.yaml";

                    configProvider.TryGet(path, out HareDuConfig config);

                    var knowledgeBaseProvider = x.Resolve<IKnowledgeBaseProvider>();

                    return new ComponentDiagnosticFactory(config.Diagnostics, knowledgeBaseProvider);
                })
                .As<IComponentDiagnosticFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var comm = x.Resolve<IBrokerCommunication>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    return new BrokerObjectFactory(comm.GetClient(settings));
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.RegisterType<BrokerCommunication>()
                .As<IBrokerCommunication>()
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