﻿// Copyright 2013-2020 Albert L. Hives
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
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Registration;
    using Snapshotting.Registration;

    public class HareDuDiagnosticsModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var provider = x.Resolve<IFileConfigProvider>();
                    string file = $"{Directory.GetCurrentDirectory()}/haredu.yaml";

                    provider.TryGet(file, out HareDuConfig config);

                    var validator = x.Resolve<IConfigValidator>();

                    if (!validator.Validate(config))
                        throw new HareDuConfigurationException($"Invalid settings in {file}.");

                    return new ScannerFactory(config.Diagnostics, x.Resolve<IKnowledgeBaseProvider>());
                })
                .As<IScannerFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var provider = x.Resolve<IFileConfigProvider>();
                    string file = $"{Directory.GetCurrentDirectory()}/haredu.yaml";

                    provider.TryGet(file, out HareDuConfig config);

                    var validator = x.Resolve<IConfigValidator>();

                    if (!validator.Validate(config))
                        throw new HareDuConfigurationException($"Invalid settings in {file}.");

                    return new BrokerObjectFactory(config.Broker);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<SnapshotFactory>()
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.RegisterType<DiagnosticsConfigProvider>()
                .As<IDiagnosticsConfigProvider>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();

            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IConfigProvider>()
                .SingleInstance();

            builder.RegisterType<HareDuConfigValidator>()
                .As<IConfigValidator>()
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